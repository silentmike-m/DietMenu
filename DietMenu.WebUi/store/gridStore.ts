import { reactive } from "@vue/composition-api";
import { GridRequest } from "~/types/core/grid";

interface GridState {
    filters: { [gridName: string]: string },
    requests: { [gridName: string]: string },
}

const state = reactive({ filters: {}, requests: {} } as GridState);

const getFilter = (gridName: string): any | null => {
    if (state.filters === undefined || !state.filters.hasOwnProperty(gridName)) {
        return null;
    }

    const filter = state.filters[gridName];
    return JSON.parse(filter);
}

const getRequest = (gridName: string): GridRequest | null => {
    if (state.requests === undefined || !state.requests.hasOwnProperty(gridName)) {
        return null;
    }

    const request = state.requests[gridName];
    return JSON.parse(request);
}

const setFilter = (gridName: string, filter: any) => {
    state.filters[gridName] = JSON.stringify(filter);
}

const setRequest = (gridName: string, request: any) => {
    state.requests[gridName] = JSON.stringify(request);
}

const gridState = {
    getFilter,
    getRequest,
    setFilter,
    setRequest,
}

export default gridState;