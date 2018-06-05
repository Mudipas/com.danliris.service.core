using Com.DanLiris.Service.Core.Lib.Services;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class Division : StandardEntity, IValidatableObject
    {
        [StringLength(100)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public string Description { get; set; }        

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.Code))
                validationResult.Add(new ValidationResult("Code is required", new List<string> { "code" }));

            if (string.IsNullOrWhiteSpace(this.Name))
                validationResult.Add(new ValidationResult("Name is required", new List<string> { "name" }));

            if(validationResult.Count.Equals(0))
            {
                /* Service Validation */
                DivisionService service = (DivisionService)validationContext.GetService(typeof(DivisionService));

                if (service.DbContext.Set<Division>().Count(r => r._IsDeleted.Equals(false) && r.Id != this.Id && r.Name.Equals(this.Name)) > 0) /* Name Unique */
                    validationResult.Add(new ValidationResult("Name already exists", new List<string> { "name" }));
            }

            return validationResult;
        }
    }
}
