using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IBFSNetwork.Models
{

    public class Questionnaire
    {
        [Key]
        public int QuestionnaireId { get; set; }

        [Display(Name = "Question")]
        [MaxLength(2048)]
        public string Question { get; set; }

        public bool Deleted { get; set; }

    }
    public class QuestionnaireAnswer
    {
        [Key]
        public int AnswerId { get; set; }

        [Required]
        [Display(Name = "Answer")]
        public string AnswerText { get; set; }

        public int QuestionnaireModelId { get; set; }

    }


    public class QuestionnaireModel
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(10)]
        public string Name { get; set; }

        public DateTime DateOn { get; set; }

        public List<QuestionnaireAnswer> QuestionnaireAnswers { get; set; }

        public QuestionnaireModel()
        {
            QuestionnaireAnswers = new List<QuestionnaireAnswer>();
        }
    }


}
