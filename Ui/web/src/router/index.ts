import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import AboutView from '../views/AboutView.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'home',
    component: AboutView
  },
  {
    path: '/family',
    name: 'family',
    component: () => import('@/views/Family/FamilyView.vue')
  },
  {
    path: '/ingredients',
    name: 'ingredients',
    component: () => import('@/views/Ingredients/IngredientsView.vue')
  },
  {
    path: '/ingredient-types',
    name: 'ingredient-types',
    component: () => import('@/views/IngredientTypes/IngredientTypesView.vue')
  },
  {
    path: '/meal-types',
    name: 'meal-types',
    component: () => import('@/views/MealTypes/MealTypesView.vue')
  },
  {
    path: '/plans',
    name: 'plans',
    component: () => import('@/views/Plans/PlansView.vue')
  },
  {
    path: '/recipes',
    name: 'recipes',
    component: () => import('@/views/Recipes/RecipesView.vue')
  },
  {
    path: '/replacements',
    name: 'replacements',
    component: () => import('@/views/Replacements/ReplacementsView.vue')
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
