export interface Recipe {
    id: string,
    carbohydrates: number,
    description: string,
    energy: number,
    fat: number,
    ingredients: RecipeIngredient[],
    meal_type_id: string | null,
    meal_type_name: string,
    name: string,
    protein: number,
}

export interface RecipesGridFilter {
    filter: string,
    ingedientFilter: string,
    mealTypeId: string | null,
}

export interface RecipeIngredient {
    id: string,
    ingredient_id: string,
    ingredient_exchanger: number,
    ingredient_name: string,
    ingredient_type_id: string,
    ingredient_type_name: string,
    quantity: number,
    unit_symbol: string,
}

export interface RecipeRow {
    id: string,
    carbohydrates: number,
    energy: number,
    fat: number,
    meal_type_name: string,
    name: string,
    protein: number,
}
