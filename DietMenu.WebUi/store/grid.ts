import { reactive } from "@vue/composition-api";

interface GridState {
    filters: { [gridName: string]: string },
}

const state = reactive({ filters: {} } as GridState);

const getFilter = (gridName: string): any | null => {

    if (state.filters === undefined || !state.filters.hasOwnProperty(gridName)) {
        return null;
    }

    const filter = state.filters[gridName];
    return JSON.parse(filter);
}

const setFilter = (gridName: string, filter: any) => {
    state.filters[gridName] = JSON.stringify(filter);
}

const gridState = {
    getFilter,
    setFilter,
}

export default gridState;