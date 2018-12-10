using System;
using System.Collections.Generic;
using Redmine.Net.Api.Types;

namespace RedmineBot.Helpers
{
    public class Generator
    {
        /// <summary>
        /// will be soon, mb
        /// </summary>
        /// <param name="userId">Redmine user id</param>
        /// <param name="taskType">Kind of type Issue by default is "Тех"</param>
        /// <param name="priority">Priority of task 2 (normal)</param>
        /// <param name="projectId">For default used 30 = 'tut ji host'</param>
        /// <param name="taskStatus">Status of task, bu default is 1 (new)</param>
        /// <param name="hours">Estimated task hours</param>
        /// <returns></returns>
        public static Issue GenerateIssue(int userId = 107, string taskType = "Тех",
            int priority = 2, int projectId = 30, IssueStatus status = IssueStatus.New, float hours = 40.0f, string subject = null)
        {
            return new Issue
            {
                AssignedTo = new IdentifiableName { Id = userId },
                Author = new IdentifiableName { Id = userId },
                CreatedOn = DateTime.Now,
                CustomFields = new List<IssueCustomField>
                {
                    new IssueCustomField { Id = 6, Multiple = false,
                        Values = new List<CustomFieldValue> { new CustomFieldValue { Info = taskType } }},
                    //Deadline
                    new IssueCustomField { Id = 13, Multiple = false,
                        Values = new List<CustomFieldValue> { new CustomFieldValue
                        {
                            Info = DateHelpers.GetLastDay().ToString(DateHelpers.DateFormat)
                        }}}
                },
                Description = string.Empty,
                DoneRatio = 0,
                Priority = new IdentifiableName { Id = priority },
                Project = new IdentifiableName { Id = projectId },
                StartDate = DateTime.Today,
                Status = new IdentifiableName { Id = (int)status },
                Subject = subject ?? GenerateTaskName(),
                Tracker = new IdentifiableName { Id = (int)Tracker.Task },
                UpdatedOn = DateTime.Now,
                DueDate = DateHelpers.GetLastDay(),
                EstimatedHours = hours
            };
        }

        /// <summary>
        /// will be soon, mb
        /// </summary>
        /// <param name="issueId">Id of task</param>
        /// <param name="hours">How many hours ure wonna spend, default is 8</param>
        /// <param name="userId">redmine userId</param>
        /// <param name="projectId">By default is 30 (tut ji host)</param>
        /// <returns></returns>
        public static TimeEntry GenerateTimeEntry(int issueId, decimal hours = 8, int userId = 107, int projectId = 30)
        {
            return new TimeEntry
            {
                Activity = new IdentifiableName { Id = (int)Activity.Another },
                CreatedOn = DateTime.Now,
                Hours = hours,
                Issue = new IdentifiableName { Id = issueId },
                Project = new IdentifiableName { Id = projectId },
                SpentOn = DateTime.Today,
                UpdatedOn = DateTime.Now,
                User = new IdentifiableName { Id = userId }
            };
        }

        private static string GenerateTaskName()
        {
            //add logic
            return "Поддержка ядра системы";
        }

    }
}
