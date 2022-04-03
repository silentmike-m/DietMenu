<template>
  <div class="table-responsive">
    <div class="text-center" v-if="isLoading">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>
    <div v-if="canAdd">
      <button
        v-if="canAdd"
        type="button"
        class="btn btn-secondary btn-sm"
        @click="onElementAdd()"
      >
        <i class="bi-plus" />
      </button>
    </div>
    <template v-if="isMobile"
      ><table class="table table-hover table-striped mobile">
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
                  @focus="$event.target.select()"
                  @keydown.enter="getData()"
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
        </thead>
        <tbody>
          <tr v-for="element in elements" :key="element.id">
            <td
              :id="element.id"
              data-bs-toggle="dropdown"
              aria-expanded="false"
            >
              <ul class="flex-content">
                <li
                  class="flex-item"
                  v-for="gridColumn in columns"
                  :key="gridColumn.value"
                  :data-label="gridColumn.title"
                >
                  <span v-if="gridColumn.type === columnTypes.boolean">
                    <span v-if="item[gridColumn.value]">TAK</span>
                    <span v-else>NIE</span>
                  </span>
                  <span v-else-if="gridColumn.type == columnTypes.date">
                    {{
                      new Date(element[gridColumn.value]).toLocaleDateString(
                        "pl-PL"
                      )
                    }}
                  </span>
                  <span v-else>
                    {{ element[gridColumn.value] }}
                  </span>
                </li>
              </ul>
            </td>
            <ul
              v-if="canDelete || canEdit"
              class="dropdown-menu table-dropdown-menu"
              :aria-labelledby="element.id"
            >
              <li>
                <a
                  class="dropdown-item"
                  href="#"
                  @click="onElementEdit(element)"
                  >Edytuj</a
                >
              </li>
              <li>
                <a
                  class="dropdown-item"
                  href="#"
                  @click="onElementDelete(element)"
                  >Usuń</a
                >
              </li>
            </ul>
          </tr>
        </tbody>
      </table></template
    >
    <template v-else>
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
                  @focus="$event.target.select()"
                  @keydown.enter="getData()"
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
            <th v-if="canDelete || canEdit" class="action-column"></th>
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
    </template>
    <div v-if="canPage">
      <nav>
        <ul class="pagination pagination-sm justify-content-end">
          <li class="page-item">
            <button
              class="btn btn-outline-secondary dropdown-toggle btn-sm"
              type="button"
              id="dropdownMenuButton1"
              data-bs-toggle="dropdown"
              aria-expanded="false"
            >
              {{ pageSize }}
            </button>
            <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">
              <li v-for="size in itemsPerPage" :key="size">
                <a
                  class="dropdown-item"
                  href="#"
                  @click="changePageSort(size)"
                  >{{ size }}</a
                >
              </li>
            </ul>
          </li>
          <li class="page-item">
            {{ pageNumber * pageSize - pageSize + 1 }} -
            {{ pageNumber * pageSize }} / {{ totalCount }}
          </li>
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
import { ComputedRef, inject, onMounted, ref, Ref } from "vue";
import { GridColumnType } from "@/models/Grid/GridColumnType";
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
  data() {
    return {
      columnTypes: GridColumnType,
    };
  },
  setup(props: any) {
    const isMobile = inject("isMobile") as ComputedRef<any>;

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
        .catch(() => {
          //ignore
        })
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
      isMobile,
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

.dropdown-item:hover {
  background-color: #a5e887;
  color: #2e2e2e !important;
}

.flex-content {
  padding: 0;
  margin: 0;
  list-style: none;
  display: flex;
  flex-wrap: wrap;
  width: 100%;
}

.flex-item {
  padding: 5px;
  width: 50%;
  font-weight: bold;
}

.mobile table tbody tr {
  max-width: 100%;
  display: block;
}

.mobile table tbody tr:nth-child(odd) {
  border-left: 6px solid #00aa95;
}

.mobile table tbody tr td {
  display: flex;
  width: 100%;
  border-bottom: 1px solid #f5f5f5 !important;
  height: auto !important;
  padding: 10px !important;
}

.flex-item:before {
  content: attr(data-label);
  padding-right: 0.5em;
  text-align: left;
  display: block;
  color: #999;
}

.page-item {
  margin-right: 5px;
  height: 35px;
  line-height: 35px;
}

.page-item:last-child {
  margin-right: 0px;
}

.table-dropdown-menu {
  background: #ffffff;
}

th {
  cursor: pointer;
}
</style>