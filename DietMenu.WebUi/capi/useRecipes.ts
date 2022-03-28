// import { Guid } from "guid-typescript";
// import { GridRequest, GridResponse } from "~/types/core/grid";
// import { Ingredient } from "~/types/ingredients";
// import { Recipe, RecipeIngredient, RecipeRow } from "~/types/recipes";
// import useRest from "./core/useRest";

// export default function useRecipes() {
//     const { post } = useRest()

//     function addIngredients(recipe: Recipe, ingredients: Ingredient[]) {
//         ingredients.forEach(i => {
//             if (recipe.ingredients.filter(i => i.ingredient_id == i.id).length <= 0) {
//                 var recipeIngredient: RecipeIngredient = {
//                     id: Guid.create().toString(),
//                     ingredient_exchanger: i.exchanger,
//                     ingredient_id: i.id,
//                     ingredient_name: i.name,
//                     ingredient_type_id: i.type_id,
//                     ingredient_type_name: i.type_name,
//                     quantity: 0,
//                     unit_symbol: i.unit_symbol
//                 }
//                 recipe.ingredients.push(recipeIngredient);
//             }
//         });

//         recipe.ingredients = recipe.ingredients.sort(function (a, b) {
//             if (a.ingredient_name < b.ingredient_name) { return -1; }
//             if (a.ingredient_name > b.ingredient_name) { return 1; }
//             return 0;
//         })
//     }

//     async function deleteRecipe(recipe: RecipeRow): Promise<boolean> {
//         try {
//             await post("Recipe/DeleteRecipe", { id: recipe.id });
//             return true;
//         } catch (e) {
//             return false;
//         }
//     }

//     async function getRecipe(id: string): Promise<Recipe> {
//         const response = await post<Recipe>("Recipe/GetRecipe", { id: id });
//         return response
//     }

//     async function getRecipesGrid(request: GridRequest, ingedientFilter: string, mealTypeId: string | null): Promise<GridResponse> {
//         const response = await post<GridResponse>("Recipe/GetRecipesGrid", { grid_request: request, ingredient_filter: ingedientFilter, meal_type_id: mealTypeId });
//         return response;
//     }

//     function getNewRecipe(): Recipe {
//         return {
//             id: Guid.create().toString(),
//             carbohydrates: 0,
//             description: '',
//             energy: 0,
//             fat: 0,
//             ingredients: [],
//             meal_type_id: null,
//             meal_type_name: '',
//             name: '',
//             protein: 0,
//         }
//     }

//     async function saveRecipe(recipe: Recipe): Promise<boolean> {
//         try {
//             var ingredients = recipe.ingredients.map(i => ({ id: i.id, ingredient_id: i.ingredient_id, quantity: i.quantity }));
//             var recipeToUpsert = {
//                 id: recipe.id,
//                 carbohydrates: recipe.carbohydrates,
//                 description: recipe.description,
//                 energy: recipe.energy,
//                 fat: recipe.fat,
//                 ingredients: ingredients,
//                 meal_type_id: recipe.meal_type_id,
//                 name: recipe.name,
//                 protein: recipe.protein,
//             };

//             await post("Recipe/UpsertRecipe", { recipe: recipeToUpsert });
//             return true;
//         } catch (e) {
//             return false;
//         }
//     }

//     return {
//         addIngredients,
//         deleteRecipe,
//         getRecipe,
//         getRecipesGrid,
//         getNewRecipe,
//         saveRecipe,
//     }
// }