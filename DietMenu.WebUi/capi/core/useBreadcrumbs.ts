import breadCrumbsState from '~/store/breadCrumbs';
import { BreadcrumbType, BreadcrumbItem } from "~/types/core/breadcrumbs";

export default function useBreadcrumbs() {

    function setBreadcrumbs(type: BreadcrumbType, data: string): void {
        const items: BreadcrumbItem[] = [];
        addItem(items, type, true, data);

        breadCrumbsState.create(items);
    }

    function addItem(list: BreadcrumbItem[], type: BreadcrumbType, disabled: boolean, data?: string): void {
        switch (+type) {
            case BreadcrumbType.Desktop:
                list.push({ text: "HOME", href: "/", disabled: disabled });
                break;
            case BreadcrumbType.DietPlans:
                addItem(list, BreadcrumbType.Desktop, false);
                list.push({ text: "PLAN", href: "/dietPlans", disabled: disabled });
                break;
            case BreadcrumbType.DietPlanRecipe:
                addItem(list, BreadcrumbType.DietPlans, false)
                list.push({ text: `${data}`, href: `/dietPlans/${data}`, disabled: disabled });
                break;
            case BreadcrumbType.Family:
                list.push({ text: "RODZINA", href: "/family", disabled: disabled });
                break;
            case BreadcrumbType.Ingredient:
                addItem(list, BreadcrumbType.Desktop, false)
                list.push({ text: "SKŁADNIKI", href: "/ingredients", disabled: disabled });
                break;
            case BreadcrumbType.IngredientDetails:
                addItem(list, BreadcrumbType.Ingredient, false)
                list.push({ text: `${data}`, href: "/", disabled: disabled });
                break;
            case BreadcrumbType.IngredientTypes:
                addItem(list, BreadcrumbType.Desktop, false)
                list.push({ text: "RODZAJE SKŁADNIKÓW", href: "/ingredientTypes", disabled: disabled });
                break;
            case BreadcrumbType.MealTypes:
                addItem(list, BreadcrumbType.Desktop, false)
                list.push({ text: "RODZAJE POSIŁKÓW", href: "/mealTypes", disabled: disabled });
                break;
            case BreadcrumbType.Recipes:
                addItem(list, BreadcrumbType.Desktop, false)
                list.push({ text: "Przepisy", href: "/recipes", disabled: disabled });
                break;
            case BreadcrumbType.RecipeDetails:
                addItem(list, BreadcrumbType.Recipes, false)
                list.push({ text: `${data}`, href: `/recipes/${data}`, disabled: disabled });
                break;
            case BreadcrumbType.Replacements:
                addItem(list, BreadcrumbType.Desktop, false)
                list.push({ text: "ZAMIENNIKI", href: "/replacements", disabled: disabled });
                break;
            case BreadcrumbType.Role:
                addItem(list, BreadcrumbType.Desktop, false)
                list.push({ text: "ROLE", href: "/roles", disabled: disabled });
                break;
            case BreadcrumbType.RoleDetails:
                addItem(list, BreadcrumbType.Role, false);
                list.push({ text: `${data}`, href: `/roles/${data}`, disabled: disabled });
                break;
            case BreadcrumbType.User:
                addItem(list, BreadcrumbType.Desktop, false)
                list.push({ text: "UŻYTKOWNICY", href: "/users", disabled: disabled });
                break;
            case BreadcrumbType.UserDetails:
                addItem(list, BreadcrumbType.User, false);
                list.push({ text: `${data}`, href: `/`, disabled: disabled });
                break;
            default: break;
        }
    }

    return {
        setBreadcrumbs,
    }
}