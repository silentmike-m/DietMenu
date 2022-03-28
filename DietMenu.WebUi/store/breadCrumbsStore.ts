import { ref } from "@vue/composition-api";
import { BreadcrumbItem } from "~/types/core/breadcrumbs";

const state = ref([] as BreadcrumbItem[]);

const create = (items: BreadcrumbItem[]): void => {
    state.value = items;
}

const get = (): BreadcrumbItem[] => {
    return state.value;
}

const breadCrumbsState = {
    create,
    get,
    state,
}

export default breadCrumbsState;