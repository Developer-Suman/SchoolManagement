using AutoMapper;
using Azure.Core;
using ES.Academics.Application.Academics.Command.AddSeatPlanning;
using ES.Academics.Application.Academics.Command.AddSubject;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Repository.HelperServices;

namespace ES.Academics.Infrastructure.ServiceImpl
{
    public class SeatPlanningServices : ISeatPlanningServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;


        public SeatPlanningServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddSeatPlannigResponse>> GenerateSeatPlanAsync(AddSeatPlanningCommand addSeatPlanningRequest)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var oldAssignments = await _unitOfWork.BaseRepository<SeatAssignment>()
                    .GetConditionalAsync(sa => sa.ExamSessionId == addSeatPlanningRequest.ExamSessionId);

                if (oldAssignments.Any())
                {
                    _unitOfWork.BaseRepository<SeatAssignment>().DeleteRange(oldAssignments.ToList());
                    await _unitOfWork.SaveChangesAsync();
                }

                var halls = await _unitOfWork.BaseRepository<ExamHall>()
                    .GetConditionalAsync(
                        eh => eh.ExamSessionId == addSeatPlanningRequest.ExamSessionId,
                        queryModifier: q => q.OrderBy(eh => eh.HallName)
                    );

                if (!halls.Any())
                    throw new Exception("No halls found for this exam session.");

                var students = await _unitOfWork.BaseRepository<StudentData>()
                    .GetConditionalFilterType(
                        s => addSeatPlanningRequest.classIds.Contains(s.ClassId),
                        q => q.Select(s => new StudentSelectDto(
                            s.Id,
                            s.FirstName + " " + s.LastName,
                            s.ClassId,
                            s.RegistrationNumber
                        ))
                    );

                var studentDetails = students.ToList();

                if (!studentDetails.Any())
                    throw new Exception("No students found for the given classIds.");

                ShuffleHelper.Shuffle(studentDetails);

                var assignments = new List<SeatAssignment>();
                var hallResponses = new List<HallSeatResponse>();

                int index = 0;

                foreach (var hall in halls)
                {
                    var hallStudents = new List<StudentSeatResponse>();

                    for (int seat = 1; seat <= hall.CapaCity; seat++)
                    {
                        if (index >= studentDetails.Count)
                            break;

                        var st = studentDetails[index];

                        assignments.Add(new SeatAssignment
                        {
                            Id = Guid.NewGuid().ToString(),
                            StudentId = st.Id,
                            ExamHallId = hall.Id,
                            ExamSessionId = addSeatPlanningRequest.ExamSessionId,
                            SeatNumber = seat
                        });

                        hallStudents.Add(new StudentSeatResponse(
                            st.Id,
                            st.FullName,
                            st.classId,
                            st.symbolNumber
                        ));

                        index++;
                    }

                    hallResponses.Add(new HallSeatResponse(
                        hall.Id,
                        hall.HallName,
                        hall.CapaCity,
                        hallStudents
                    ));

                    if (index >= studentDetails.Count)
                        break;
                }

                if (index < studentDetails.Count)
                    throw new Exception("Not enough hall capacity to allocate all students.");

                await _unitOfWork.BaseRepository<SeatAssignment>().AddRange(assignments);
                await _unitOfWork.SaveChangesAsync();

                var result = new AddSeatPlannigResponse(
                    addSeatPlanningRequest.ExamSessionId,
                    studentDetails.Count,
                    hallResponses
                );

                scope.Complete();
                return Result<AddSeatPlannigResponse>.Success(result);
            }
            catch (Exception ex)
            {
                scope.Dispose();
                throw new Exception("An error occurred while generating seat planning.", ex);
            }
        }

    }
}
