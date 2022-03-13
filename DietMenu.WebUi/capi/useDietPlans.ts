import { Guid } from "guid-typescript";
import { DietPlanRecipe, DietPlanRecipeIngredient, DietPlanRecipeRow } from "~/types/dietPlans";
import { Ingredient } from "~/types/ingredients";
import useRest from "./core/useRest";
import useMealTypes from "./useMealTypes";
import useRecipes from "./useRecipes";

export default function useDietPlans() {
    const { getMealTypes } = useMealTypes();
    const { getRecipe } = useRecipes();
    const { post } = useRest();

    function addIngredients(recipe: DietPlanRecipe, ingredients: Ingredient[]) {
        ingredients.forEach(i => {
            if (recipe.ingredients.filter(i => i.ingredient_id == i.id).length <= 0) {
                var recipeIngredient: DietPlanRecipeIngredient = {
                    id: Guid.create().toString(),
                    ingredient_exchanger: i.exchanger,
                    ingredient_id: i.id,
                    ingredient_name: i.name,
                    ingredient_type_id: i.type_id,
                    ingredient_type_name: i.type_name,
                    quantity: 0,
                    unit_symbol: i.unit_symbol
                }
                recipe.ingredients.push(recipeIngredient);
            }
        });

        recipe.ingredients = recipe.ingredients.sort(function (a, b) {
            if (a.ingredient_name < b.ingredient_name) { return -1; }
            if (a.ingredient_name > b.ingredient_name) { return 1; }
            return 0;
        })
    }

    async function deleteDietPlanRecipe(recipe: DietPlanRecipeRow): Promise<boolean> {
        try {
            await post("DietPlan/DeleteDietPlanRecipe", { id: recipe.id });
            return true;
        } catch (e) {
            return false;
        }
    }

    async function fillDietPlanRecipe(sourceId: string, target: DietPlanRecipe) {
        var source = await getRecipe(sourceId);

        target.carbohydrates = source.carbohydrates;
        target.description = source.description;
        target.energy = source.energy;
        target.fat = source.fat;
        target.ingredients = [];
        target.name = source.name;
        target.protein = source.protein;

        source.ingredients.forEach(i => {
            var recipeIngredient: DietPlanRecipeIngredient = {
                id: Guid.create().toString(),
                ingredient_exchanger: i.ingredient_exchanger,
                ingredient_id: i.ingredient_id,
                ingredient_name: i.ingredient_name,
                ingredient_type_id: i.ingredient_type_id,
                ingredient_type_name: i.ingredient_type_name,
                quantity: i.quantity,
                unit_symbol: i.unit_symbol
            }
            target.ingredients.push(recipeIngredient);
        });
    }

    async function getDietPlanRecipes(dateFrom: Date, dateTo: Date): Promise<DietPlanRecipeRow[]> {
        const response = await post<DietPlanRecipeRow[]>("DietPlan/GetDietPlanRecipes", { date_from: dateFrom, date_to: dateTo });
        return response
    }

    async function getDietPlanRecipe(id: string): Promise<DietPlanRecipe> {
        const response = await post<DietPlanRecipe>("DietPlan/GetDietPlanRecipe", { id: id });
        return response
    }

    async function getNewDietPlanRecipe(date: Date, mealTypeId: string): Promise<DietPlanRecipe> {
        const mealTypes = await getMealTypes();
        const mealType = mealTypes.filter(i => i.id === mealTypeId)[0];

        return {
            id: Guid.create().toString(),
            carbohydrates: 0,
            date: date,
            description: '',
            energy: 0,
            fat: 0,
            ingredients: [],
            meal_type_id: mealType.id,
            meal_type_name: mealType.name,
            name: '',
            protein: 0,
        }
    }

    async function saveDietPlanRecipe(recipe: DietPlanRecipe): Promise<boolean> {
        try {
            var ingredients = recipe.ingredients.map(i => ({ id: i.id, ingredient_id: i.ingredient_id, quantity: i.quantity }));
            var recipeToUpsert = {
                id: recipe.id,
                carbohydrates: recipe.carbohydrates,
                date: recipe.date,
                description: recipe.description,
                energy: recipe.energy,
                fat: recipe.fat,
                ingredients: ingredients,
                meal_type_id: recipe.meal_type_id,
                name: recipe.name,
                protein: recipe.protein,
            };

            await post("DietPlan/UpsertDietPlanRecipe", { recipe: recipeToUpsert });
            return true;
        } catch (e) {
            return false;
        }
    }

    return {
        addIngredients,
        deleteDietPlanRecipe,
        fillDietPlanRecipe,
        getDietPlanRecipe,
        getDietPlanRecipes,
        getNewDietPlanRecipe,
        saveDietPlanRecipe,
    }
}