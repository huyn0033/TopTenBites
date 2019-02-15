using FluentValidation;
using System.Linq;
using TopTenBites.Web.Api.v1.ApiModels;
using TopTenBites.Web.ApplicationCore.Interfaces;

namespace TopTenBites.Web.Api.v1.Validation
{
    public class MenuItemViewModelValidator : AbstractValidator<MenuItemApiModel>
    {
        private IBusinessService _businessService;

        public MenuItemViewModelValidator(IBusinessService businessService)
        {
            _businessService = businessService;

            RuleSet("Insert", () =>
            {
                RuleFor(x => x.Id).Empty();
                RuleFor(x => x).Must(x => !IsDuplicate(x)).WithMessage("Name must be unique");
                ExecuteCommonRules();
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x).Must(x => !IsDuplicate(x)).WithMessage("Name must be unique");
                ExecuteCommonRules();
            });
        }

        private void ExecuteCommonRules()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(32);
            RuleFor(x => x.BusinessId).NotEmpty();
        }

        private bool IsDuplicate(MenuItemApiModel menuItemVM)
        {
            var menuItems = _businessService.GetByBusinessId(menuItemVM.BusinessId)?.MenuItems;
            if (menuItems != null && menuItems.Count > 0)
            {
                if (menuItemVM.Id == null)
                    return menuItems.Any(x =>
                                        (x.Name == menuItemVM.Name));
                else
                    return menuItems.Any(x => (x.MenuItemId != menuItemVM.Id) &&
                                        (x.Name == menuItemVM.Name));
            }

            return false;
        }
    }
}
