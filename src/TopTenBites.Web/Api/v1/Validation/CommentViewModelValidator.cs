using FluentValidation;
using TopTenBites.Web.Api.v1.ApiModels;

namespace TopTenBites.Web.Api.v1.Validation
{
    public class CommentViewModelValidator : AbstractValidator<CommentApiModel>
    {
        public CommentViewModelValidator()
        {
            RuleSet("Insert", () =>
            {
                RuleFor(x => x.Id).Empty();
                ExecuteCommonRules();
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id).NotEmpty();
                ExecuteCommonRules();
            });
        }

        private void ExecuteCommonRules()
        {
            RuleFor(x => x.Text).NotEmpty().MaximumLength(140);
            RuleFor(x => x.Sentiment).IsInEnum();
            RuleFor(x => x.MenuItemId).NotEmpty();
        }

    }
}
