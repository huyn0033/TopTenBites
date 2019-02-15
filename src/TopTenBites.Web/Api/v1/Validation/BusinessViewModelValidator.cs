using FluentValidation;
using System.Linq;
using TopTenBites.Web.Api.v1.ApiModels;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.Api.v1.Validation
{
    public class BusinessViewModelValidator : AbstractValidator<BusinessApiModel>
    {
        private IBusinessService _businessService;

        public BusinessViewModelValidator(IBusinessService businessService)
        {
            _businessService = businessService;

            RuleSet("Insert", () =>
            {
                RuleFor(x => x.Id).Empty();
                RuleFor(x => x).Must(x => !IsDuplicate(x)).WithMessage("YelpBusinessId and YelpBusinessAlias must be unique");
                ExecuteCommonRules();
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x).Must(x => !IsDuplicate(x)).WithMessage("YelpBusinessId and YelpBusinessAlias must be unique");
                ExecuteCommonRules();
            });
        }

        private void ExecuteCommonRules()
        {
            RuleFor(x => x.YelpBusinessId).NotEmpty();
            RuleFor(x => x.YelpBusinessAlias).NotEmpty();
        }

        private bool IsDuplicate(BusinessApiModel businessVM)
        {
            var businesses = _businessService.GetAll();
            if (businessVM.Id == null)
                return businesses.Any(x => 
                                    (x.YelpBusinessId == businessVM.YelpBusinessId || x.YelpBusinessAlias == businessVM.YelpBusinessAlias));
            else
                return businesses.Any(x => (x.BusinessId != businessVM.Id) && 
                                    (x.YelpBusinessId == businessVM.YelpBusinessId || x.YelpBusinessAlias == businessVM.YelpBusinessAlias));
        }

    }
}
