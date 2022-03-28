// import { Guid } from "guid-typescript";
// import { GridRequest, GridResponse } from "~/types/core/grid";
// import { Ingredient } from "~/types/ingredients";
// import useRest from "./core/useRest";

// export default function useIngredients() {
//     const { post } = useRest()

//     async function deleteIngredient(ingredient: Ingredient): Promise<boolean> {
//         try {
//             await post("Ingredient/DeleteIngredient", { id: ingredient.id });
//             return true;
//         } catch (e) {
//             return false;
//         }
//     }

//     async function getIngredient(id: string): Promise<Ingredient> {
//         const response = await post<Ingredient>("Ingredient/GetIngredient", { id: id });
//         return response;
//     }

//     async function getIngredientTypesGrid(request: GridRequest): Promise<GridResponse> {
//         const response = await post<GridResponse>("IngredientTypes/GetIngredientTypesGrid", { grid_request: request });
//         return response;
//     }

//     async function getIngredientsGrid(request: GridRequest): Promise<GridResponse> {
//         const response = await post<GridResponse>("Ingredients/GetIngredientsGrid", { grid_request: request });
//         return response;
//     }

//     function getNewIngredient(): Ingredient {
//         return {
//             id: Guid.create().toString(),
//             exchanger: 1,
//             is_system: false,
//             name: '',
//             type_id: '',
//             type_name: '',
//             unit_symbol: '',
//         }
//     }

//     async function getReplacementsGrid(exchanger: number, quantity: number, request: GridRequest, typeId: string): Promise<GridResponse> {
//         const response = await post<GridResponse>("Ingredient/GetIngredientReplacementsGrid",
//             {
//                 exchanger: exchanger,
//                 grid_request: request,
//                 quantity: quantity,
//                 type_id: typeId
//             });
//         return response;
//     }

//     async function saveIngredient(ingredient: Ingredient): Promise<boolean> {
//         try {
//             await post("Ingredient/UpsertIngredient", { ingredient: ingredient });
//             return true;
//         } catch (e) {
//             return false;
//         }
//     }

//     return {
//         deleteIngredient,
//         getIngredient,
//         getIngredientTypesGrid,
//         getIngredientsGrid,
//         getNewIngredient,
//         getReplacementsGrid,
//         saveIngredient,
//     }
// }