﻿using AppTracker150Server.Data;
using AppTracker150Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTracker150Server.Services
{
    public class ApplicationService
    {
        private readonly Guid _userId;
        public ApplicationService(Guid userId)
        {
            _userId = userId;
        }
        public bool CreateApplication(ApplicationCreate model)
        {
            var entity =
            new Application()
            {
                StudentId = _userId,
                DateCreatedUtc = DateTimeOffset.Now,
                ApplicationStatus = model.ApplicationStatus,
                CompanyName = model.CompanyName,
                PositionName = model.PositionName,
                JobLink = model.JobLink,
                JobLocation = model.JobLocation,
                Research = model.Research,
                Contacts = model.Contacts,
                SourceOfPosting = model.SourceOfPosting
            };
            using (var context = new ApplicationDbContext())
            {
                context.Applications.Add(entity);
                return context.SaveChanges() == 1;
            }
        }
        public IEnumerable<ApplicationListItem> GetApplications()
        {
            using (var context = new ApplicationDbContext())
            {
                var query =
                    from application in context.Applications
                    join student in context.Student on application.StudentId equals student.StudentId into studentApplications
                    from item in studentApplications.DefaultIfEmpty()
                    select new ApplicationListItem()
                    {
                        ApplicationId = application.Id,
                        CompanyName = application.CompanyName,
                        PositionName = application.PositionName,
                        ApplicationEnum = application.ApplicationStatus,
                        DateCreatedUtc = application.DateCreatedUtc,
                        StudentName = item.FirstName + " " + item.LastName,
                        StudentId = item.StudentId
                    };
                var result = query
                    .ToArray().Select(ali =>
                                 new ApplicationListItem()
                                 {
                                     ApplicationId = ali.ApplicationId,
                                     CompanyName = ali.CompanyName,
                                     PositionName = ali.PositionName,
                                     ApplicationStatus = ali.ApplicationEnum.ToString(),
                                     DateCreatedUtc = ali.DateCreatedUtc,
                                     StudentName = ali.StudentName,
                                     StudentId = ali.StudentId
                                 });

                return result;
            }
        }
        public IEnumerable<ApplicationListItem> GetApplications(Guid id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity =
                      context.Applications
                             .Where(e => e.StudentId == id)
                             .Select(
                                e =>
                                    new ApplicationListItem()
                                    {
                                        ApplicationId = e.Id,
                                        CompanyName = e.CompanyName,
                                        PositionName = e.PositionName,
                                        ApplicationStatus = e.ApplicationStatus.ToString(),
                                        DateCreatedUtc = e.DateCreatedUtc
                                    });
                return entity.ToArray();
            }
        }
        public ApplicationDetail GetApplicationById(int id, Guid? _studentId = null)
        {
            Guid studentId; 
            if (_studentId == null)
            {
                studentId = _userId;
            }
            else
            {
                studentId = (Guid)_studentId;
            }
            using (var context = new ApplicationDbContext())
            {
                var entity =
                    context.Applications
                            .Single(e => e.Id == id && e.StudentId == studentId);
                return
                    new ApplicationDetail
                    {
                        Id = entity.Id,
                        CompanyName = entity.CompanyName,
                        ApplicationStatus = entity.ApplicationStatus.ToString(),
                        Contacts = entity.Contacts,
                        DateCreatedUtc = entity.DateCreatedUtc,
                        DateModifiedUtc = entity.DateModifiedUtc,
                        JobLink = entity.JobLink,
                        JobLocation = entity.JobLocation,
                        PositionName = entity.PositionName,
                        Research = entity.Research,
                        SourceOfPosting = entity.SourceOfPosting,
                        StudentId = studentId
                    };
            }
        }
        public bool UpdateApplication(ApplicationEdit model)
        {
            using(var context = new ApplicationDbContext())
            {
                var entity =
                       context
                              .Applications
                              .Single(e => e.Id == model.Id && e.StudentId == _userId);
                entity.CompanyName = model.CompanyName;
                entity.ApplicationStatus = model.ApplicationStatus;
                entity.Contacts = model.Contacts;
                entity.DateModifiedUtc = DateTimeOffset.Now;
                entity.JobLink = model.JobLink;
                entity.JobLocation = model.JobLocation;
                entity.PositionName = model.PositionName;
                entity.Research = model.Research;
                entity.SourceOfPosting = model.SourceOfPosting;
                entity.Id = model.Id;
                entity.StudentId = _userId;

                return context.SaveChanges() == 1;
            }
        }
        public bool DeleteApplication(int applicationid)
        {
            using(var context = new ApplicationDbContext())
            {
                var entity =
                    context.Applications
                           .Single(e => e.Id == applicationid && e.StudentId == _userId);
                context.Applications.Remove(entity);

                return context.SaveChanges() == 1;
            }
        }

    }
}
