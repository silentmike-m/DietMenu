import { reactive } from "@vue/composition-api";

interface EditorState {
    id: string,
    data: any,
}

const state = reactive({} as EditorState);

const getData = (id: string): any => {
    if (state.id === id) {
        return state.data;
    }

    return null;
}

const setData = (id: string, data: any): void => {
    state.id = id;
    state.data = data;
}

const editorState = {
    getData,
    setData,
}

export default editorState;