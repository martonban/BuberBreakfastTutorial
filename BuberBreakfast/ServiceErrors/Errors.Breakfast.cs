using ErrorOr;

namespace BuberBreakfast.ServiceErrors;

public static class Errors
{
    public static class Breakfast
    {
        public static Error NotFound => Error.NotFound(
                 code: "Breakfast.NotFound",
                 description: "Breakfast not found"
             );

        public static Error InvalidName => Error.NotFound(
            code: "Breakfast.InvalidName",
            description: $"Breakfast name must be at least {Models.Breakfast.MinNameLength} character long and most {Models.Breakfast.MaxNameLength} character long"
        );
        
        public static Error InvalidDescription => Error.NotFound(
            code: "Breakfast.InvaliDescription",
            description: $"Breakfast description must be at least {Models.Breakfast.MinDescriptionLength} character long and most {Models.Breakfast.MaxDescriptionLength} character long"
        );
    }
}