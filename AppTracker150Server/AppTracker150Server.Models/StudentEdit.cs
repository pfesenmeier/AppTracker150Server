﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTracker150Server.Models
{
    public class StudentEdit
    {
        [Required]
        public Guid StudentId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string CohortId { get; set; }

        [Required]
        public string ResumeLink { get; set; }

        [Required]
        public string LinkedInLink { get; set; }

        [Required]
        public string PortfolioLink { get; set; }

        [Required]
        public string GitHub { get; set; }
    }
}