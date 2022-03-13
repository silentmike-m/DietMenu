export enum BreadcrumbType {
    Desktop,
    DietPlans,
    DietPlanRecipe,
    Family,
    Ingredient,
    IngredientDetails,
    IngredientTypes,
    Recipes,
    RecipeDetails,
    Replacements,
    Role,
    RoleDetails,
    User,
    UserDetails,
}

export class BreadcrumbItem {
    constructor(
        public text: string,
        public href: string,
        public disabled: boolean
    ) {

    }
}