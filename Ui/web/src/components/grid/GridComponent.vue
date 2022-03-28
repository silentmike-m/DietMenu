<template>
  <div class="table-responsive">
    <div class="text-center" v-if="isLoading">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>
    <table class="table table-hover table-striped">
      <thead>
        <tr v-if="canFilter">
          <td :colspan="columns.length + 1">
            <div class="input-group mb-3">
              <input
                type="text"
                class="form-control"
                placeholder="szukaj ..."
                aria-label="szukaj ..."
                v-model="filter"
              />
              <button
                class="btn btn-outline-secondary"
                type="button"
                @click="clearFilter()"
              >
                <i class="bi-x" />
              </button>
              <button
                class="btn btn-outline-secondary"
                type="button"
                @click="getData()"
              >
                <i class="bi-search" />
              </button>
            </div>
          </td>
        </tr>
        <tr>
          <th
            scope="col"
            v-for="gridColumn in columns"
            :key="gridColumn.value"
            @click="changeSort(gridColumn.value)"
          >
            <template v-if="sortBy == gridColumn.value">
              <i v-if="sortDescending" class="bi-arrow-up"></i>
              <i v-else class="bi-arrow-down"></i>
            </template>
            {{ gridColumn.title }}
          </th>
          <th v-if="canAdd || canDelete || canEdit" class="action-column">
            <button
              v-if="canAdd"
              type="button"
              class="btn btn-secondary btn-sm"
              @click="onElementAdd()"
            >
              <i class="bi-plus" />
            </button>
          </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="element in elements" :key="element.id">
          <td v-for="gridColumn in columns" :key="gridColumn.value">
            {{ element[gridColumn.value] }}
          </td>
          <td v-if="canDelete || canEdit" class="action-column">
            <button
              v-if="canEdit"
              type="button"
              class="btn btn-secondary btn-sm"
              @click="onElementEdit(element)"
            >
              <i class="bi-pencil" />
            </button>
            <button
              v-if="canDelete"
              type="button"
              class="btn btn-outline-danger btn-sm"
              @click="onElementDelete(element)"
            >
              <i class="bi-x" />
            </button>
          </td>
        </tr>
      </tbody>
    </table>
    <div v-if="canPage">
      <select
        class="form-select form-select-sm"
        aria-label=".form-select-sm example"
      >
        <option
          v-for="size in itemsPerPage"
          :selected="pageSize === size"
          :key="size"
          @click="changePageSort(size)"
        >
          {{ size }}
        </option>
      </select>
      <span
        >{{ pageNumber * pageSize - pageSize + 1 }} -
        {{ pageNumber * pageSize }} / {{ totalCount }}</span
      >
      <nav>
        <ul class="pagination pagination-sm">
          <li class="page-item">
            <button
              class="btn btn-outline-secondary btn-sm"
              type="button"
              :disabled="pageNumber === 1"
              @click="getFirstPage"
            >
              |&lt;
            </button>
          </li>
          <li class="page-item">
            <button
              class="btn btn-outline-secondary btn-sm"
              type="button"
              :disabled="pageNumber === 1"
              @click="getPreviousPage()"
            >
              &lt;
            </button>
          </li>
          <li class="page-item">
            <button
              class="btn btn-outline-secondary btn-sm"
              type="button"
              disabled
            >
              {{ pageNumber }}
            </button>
          </li>
          <li class="page-item">
            <button
              class="btn btn-outline-secondary btn-sm"
              type="button"
              :disabled="pageNumber * pageSize >= totalCount"
              @click="getNextPage()"
            >
              &gt;
            </button>
          </li>
          <li class="page-item">
            <button
              class="btn btn-outline-secondary btn-sm"
              type="button"
              :disabled="pageNumber * pageSize >= totalCount"
              @click="getLastPage()"
            >
              &gt;|
            </button>
          </li>
        </ul>
      </nav>
    </div>
  </div>
</template>

<script lang="ts">
import { onMounted, ref, Ref } from "vue";
import { GridRequest } from "@/models/Grid/GridRequest";
import { GridResponse } from "@/models/Grid/GridResponse";

export default {
  props: {
    canAdd: Boolean,
    canDelete: Boolean,
    canEdit: Boolean,
    canFilter: Boolean,
    canPage: Boolean,
    canSort: Boolean,
    columns: Array,
    getGridData: Function,
    onElementAdd: Function,
    onElementEdit: Function,
    onElementDelete: Function,
  },
  setup(props: any) {
    const elements: Ref<any[]> = ref([]);
    const filter: Ref<string> = ref("");
    const isLoading: Ref<boolean> = ref(false);
    const itemsPerPage: Ref<number[]> = ref([5, 10, 15, 50, 100]);
    const pageNumber: Ref<number> = ref(1);
    const pageSize: Ref<number> = ref(10);
    const sortBy: Ref<string> = ref("");
    const sortDescending: Ref<boolean> = ref(false);
    const totalCount: Ref<number> = ref(0);

    onMounted(() => {
      if (props.canSort) {
        sortBy.value = props.columns[0].value;
        sortDescending.value = false;
      }

      getData();
    });

    const changePageSort = (size: number) => {
      if (size === pageSize.value || !props.canPage) {
        return;
      }

      pageNumber.value = 1;
      pageSize.value = size;

      getData();
    };

    const changeSort = (header: string) => {
      if (!props.canSort) {
        return;
      }

      if (sortBy.value === header) {
        sortDescending.value = !sortDescending.value;
      } else {
        sortBy.value = header;
        sortDescending.value = false;
      }

      getData();
    };

    const clearFilter = () => {
      filter.value = "";

      getData();
    };

    const getData = () => {
      const request: GridRequest = {
        filter: filter.value,
        is_descending: sortDescending.value,
        is_paged: true,
        order_by: sortBy.value,
        page_number: pageNumber.value - 1,
        page_size: pageSize.value,
      };

      isLoading.value = true;

      (props as any)
        .getGridData(request)
        .then((response: GridResponse) => processResponse(response))
        .finally(() => (isLoading.value = false));
    };

    const getFirstPage = () => {
      if (!props.canPage || pageNumber.value === 1) {
        return;
      }

      pageNumber.value = 1;

      getData();
    };

    const getLastPage = () => {
      if (
        !props.canPage ||
        pageNumber.value * pageSize.value >= totalCount.value
      ) {
        return;
      }

      pageNumber.value = totalCount.value / pageSize.value;

      getData();
    };

    const getNextPage = () => {
      if (
        !props.canPage ||
        pageNumber.value * pageSize.value >= totalCount.value
      ) {
        return;
      }

      pageNumber.value++;

      getData();
    };

    const getPreviousPage = () => {
      if (!props.canPage || pageNumber.value === 1) {
        return;
      }

      pageNumber.value--;

      getData();
    };

    const processResponse = (gridResponse: GridResponse) => {
      elements.value = gridResponse.elements;
      totalCount.value = gridResponse.count;
    };

    return {
      elements,
      filter,
      isLoading,
      itemsPerPage,
      pageNumber,
      pageSize,
      sortBy,
      sortDescending,
      totalCount,
      changePageSort,
      changeSort,
      clearFilter,
      getData,
      getFirstPage,
      getLastPage,
      getNextPage,
      getPreviousPage,
    };
  },
};
</script>

<style scoped>
.action-column {
  width: 110px;
  text-align: center !important;
}

.btn-outline-secondary {
  color: #00aa95;
}

.page-item {
  margin-right: 5px;
}

.page-item:last-child {
  margin-right: 0px;
}

th {
  cursor: pointer;
}
</style>