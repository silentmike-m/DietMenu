<template>
  <v-card>
    <v-card-text>
      <v-btn v-if="canAdd" icon @click="onRowAdd()" color="primary">
        <v-icon>mdi-plus-thick</v-icon>
      </v-btn>
      <v-menu v-if="menuButtons && $vuetify.breakpoint.smAndUp">
        <template v-slot:activator="{ on, attrs }">
          <v-btn icon v-bind="attrs" v-on="on">
            <v-icon small>mdi-dots-vertical</v-icon>
          </v-btn>
        </template>
        <v-list>
          <v-list-item
            v-for="button in menuButtons"
            :key="button.id"
            @click="button.onClick(selected)"
          >
            <v-list-item-title>
              <v-icon>{{ button.icon }}</v-icon>
              {{ button.text }}
            </v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
      <v-menu v-if="!$vuetify.breakpoint.smAndUp && canSort" offset-y>
        <template v-slot:activator="{ on, attrs }">
          <v-btn icon v-bind="attrs" v-on="on">
            <v-icon small>mdi-dots-vertical</v-icon>
          </v-btn>
        </template>
        <v-list>
          <v-list-item
            v-for="col in sortableColumns"
            :key="col.value"
            @click="changeSort(col.value)"
          >
            <v-list-item-title>
              {{ col.text }}

              <template v-if="sortBy == col.value">
                <v-icon v-if="sortDesc" small>mdi-arrow-down</v-icon>
                <v-icon v-else small>mdi-arrow-up</v-icon>
              </template>
            </v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
      <v-layout column style="padding-top: 5px">
        <p v-if="canMultiSelect">
          Zaznaczono: {{ selected.length }}
          <v-btn icon @click="clearSelection()" color="primary">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </p>
        <v-data-table
          class="elevation-1"
          :class="{ mobile: !$vuetify.breakpoint.smAndUp }"
          :search="search"
          :headers="columns"
          :items="elements"
          :loading="isLoading"
          :server-items-length="totalCount"
          :hide-default-header="!$vuetify.breakpoint.smAndUp"
          :page.sync="pageNumber"
          :items-per-page.sync="pageSize"
          :must-sort="canSort"
          :disable-sort="!canSort"
          :sort-by.sync="sortBy"
          :sort-desc.sync="sortDesc"
          v-model="selected"
          :single-select="!canMultiSelect || !$vuetify.breakpoint.smAndUp"
          :show-select="canSelect"
          :hide-default-footer="!canPage"
          :footer-props="{
            showFirstLastPage: true,
            showCurrentPage: true,
            itemsPerPageOptions: [5, 10, 15, 50, 100],
            itemsPerPageText: '',
          }"
        >
          <template v-slot:top>
            <v-text-field
              v-model="search"
              label="szukaj ..."
              class="mx-4"
              hide-details
              clearable
              v-if="canFilter"
              @keydown.enter="getGridData()"
              @focus="$event.target.select()"
              @click:clear="clearSearch()"
            >
              ><v-icon slot="append" @click="getGridData()"
                >mdi-magnify</v-icon
              ></v-text-field
            >
          </template>

          <template
            v-if="$vuetify.breakpoint.smAndUp"
            v-slot:item="{ item, isSelected }"
          >
            <tr>
              <td v-if="canSelect" class="select-column">
                <v-simple-checkbox
                  :value="isSelected"
                  @click="select(item, isSelected)"
                ></v-simple-checkbox>
              </td>
              <td
                v-for="col in columns"
                :key="col.value"
                @click="
                  canMultiSelect ? select(item, isSelected) : focusRow(item)
                "
                v-bind:style="canSelect || canEdit ? 'cursor: pointer' : ''"
              >
                <span v-if="col.type === columnTypes.boolean">
                  <span v-if="item[col.value]">TAK</span>
                  <span v-else>NIE</span>
                </span>

                <span v-else-if="col.type == columnTypes.date">
                  {{ new Date(item[col.value]).toLocaleDateString("pl-PL") }}
                </span>
                <span v-else>
                  {{ item[col.value] }}
                </span>
              </td>
              <td class="action-column" v-if="canEdit | canShow | canDelete">
                <v-btn
                  icon
                  v-if="canEdit | canShow"
                  @click="canEdit | canShow ? onRowEdit(item) : {}"
                >
                  <v-icon small>mdi-magnify</v-icon>
                </v-btn>
                <v-btn
                  icon
                  v-if="canDelete"
                  @click="canDelete ? onRowDelete(item) : {}"
                >
                  <v-icon small>mdi-delete</v-icon>
                </v-btn>
              </td>
            </tr>
          </template>
          <template v-else v-slot:item="{ item }">
            <tr>
              <v-menu>
                <template v-slot:activator="{ on }">
                  <td @contextmenu.prevent="on.click" @click="focusRow(item)">
                    <ul class="flex-content">
                      <li
                        class="flex-item"
                        v-for="col in columns"
                        :key="col.value"
                        :data-label="col.text"
                      >
                        <span v-if="col.type === columnTypes.boolean">
                          <span v-if="item[col.value]">TAK</span>
                          <span v-else>NIE</span>
                        </span>
                        <span v-else-if="col.type == columnTypes.date">
                          {{
                            new Date(item[col.value]).toLocaleDateString(
                              "pl-PL"
                            )
                          }}
                        </span>
                        <span v-else>
                          {{ item[col.value] }}
                        </span>
                      </li>
                    </ul>
                  </td>
                </template>
                <v-list>
                  <v-list-item
                    v-if="canEdit || canShow"
                    @click="canEdit | canShow ? onRowEdit(item) : {}"
                  >
                    <v-list-item-title>
                      <v-icon small>mdi-magnify</v-icon>
                      Edytuj
                    </v-list-item-title>
                  </v-list-item>
                  <v-list-item
                    v-if="canDelete"
                    @click="canDelete ? onRowDelete(item) : {}"
                  >
                    <v-list-item-title>
                      <v-icon small>mdi-delete</v-icon>
                      Usuń
                    </v-list-item-title>
                  </v-list-item>
                  <v-list-item
                    v-for="button in menuButtons"
                    :key="button.id"
                    @click="button.onClick(item)"
                  >
                    <v-list-item-title>
                      <v-icon small>{{ button.icon }}</v-icon>
                      {{ button.text }}
                    </v-list-item-title>
                  </v-list-item>
                </v-list>
              </v-menu>
            </tr>
          </template>
          <template slot="no-data" :value="true">
            <v-alert v-if="search">
              Brak danych dla wyszukiwania "{{ search }}".
            </v-alert>
            <v-alert v-else>Brak danych.</v-alert>
          </template>
          <template slot="no-results"><v-alert>Brak danych.</v-alert></template>
        </v-data-table>
      </v-layout>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";
import { onMounted, Ref, ref, watch } from "@nuxtjs/composition-api";
import {
  GridColumn,
  GridColumnType,
  GridRequest,
  GridResponse,
} from "~/types/core/grid";
import gridState from "~/store/gridStore";

export default Vue.extend({
  name: "Grid",
  props: {
    canAdd: Boolean,
    canDelete: Boolean,
    canEdit: Boolean,
    canFilter: Boolean,
    canMultiSelect: Boolean,
    canPage: Boolean,
    canSelect: Boolean,
    canSort: Boolean,
    canShow: Boolean,
    columns: Array,
    getData: Function,
    id: Number,
    menuButtons: Array,
    name: String,
    onRowAdd: Function,
    onRowEdit: Function,
    onRowDelete: Function,
    onFocusRow: Function,
  },
  data() {
    return {
      columnTypes: GridColumnType,
    };
  },
  setup(props) {
    const elements: Ref<any[]> = ref([]);
    const isLoading: Ref<boolean> = ref(true);
    const pageNumber: Ref<number> = ref(1);
    const pageSize: Ref<number> = ref(10);
    const search: Ref<string> = ref("");
    const selected: Ref<any[]> = ref([]);
    const sortableColumns: Ref<GridColumn[]> = ref([]);
    const sortBy: Ref<string> = ref("");
    const sortDesc: Ref<Boolean> = ref(false);
    const totalCount: Ref<number> = ref(0);

    // watch(pageNumber, () => getGridData());
    // watch(pageSize, () => getGridData());
    // watch(
    //   () => sortBy.value,
    //   () => getGridData()
    // );
    // watch(
    //   () => sortDesc.value,
    //   () => getGridData()
    // );
    watch(
      () => props.id,
      () => getGridData()
    );

    onMounted(() => {
      isLoading.value = true;

      let columns = props.columns as GridColumn[];

      sortableColumns.value = columns.filter((i) => i.sortable);

      const request = gridState.getRequest(props.name as string);

      if (request) {
        search.value = request.filter;
        // sortBy.value = request.order_by;
        // sortDesc.value = request.is_descending;
        pageNumber.value = request.page_number + 1;
        pageSize.value = request.page_size;
      } else {
        sortBy.value = columns[0].value;
      }

      getGridData();

      isLoading.value = false;
    });

    const changeSort = (header: string): void => {
      if (!props.canSort) {
        return;
      }

      if (sortBy.value === header) {
        sortDesc.value = !sortDesc.value;
      } else {
        sortBy.value = header;
        sortDesc.value = false;
      }

      getGridData();
    };

    const clearSearch = (): void => {
      search.value = "";
      getGridData();
    };

    const clearSelection = (): void => {
      selected.value = [];
    };

    const focusRow = (row: any): void => {
      selected.value = [row];

      if (props.onFocusRow) {
        (props as any).onFocusRow();
      } else if (props.onRowEdit) {
        (props as any).onRowEdit(row);
      }
    };

    function getGridData() {
      let request: GridRequest = {
        filter: search.value,
        order_by: sortBy.value,
        is_descending: sortDesc.value,
        is_paged: true,
        page_number: pageNumber.value - 1,
        page_size: pageSize.value,
      };

      isLoading.value = true;

      gridState.setRequest(props.name as string, request);

      (props as any)
        .getData(request)
        .then((result: GridResponse) => processResponse(result))
        .finally(() => (isLoading.value = false));
    }

    function processResponse(response: GridResponse) {
      elements.value = response.elements;
      totalCount.value = response.count;
    }

    const select = (row: any, isSelected: boolean): void => {
      if (isSelected) {
        selected.value = selected.value.filter((i) => i !== row);
      } else {
        selected.value.push(row);
      }
    };

    return {
      changeSort,
      clearSearch,
      clearSelection,
      elements,
      focusRow,
      getGridData,
      isLoading,
      pageNumber,
      pageSize,
      search,
      select,
      selected,
      sortableColumns,
      sortBy,
      sortDesc,
      totalCount,
    };
  },
});
</script>

<style scoped>
@media screen and (max-width: 768px) {
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

  .mobile table tbody tr td ul li:before {
    content: attr(data-label);
    padding-right: 0.5em;
    text-align: left;
    display: block;
    color: #999;
  }
  .mobile table tbody tr td ul li .v-input--selection-controls {
    margin-top: 0;
  }
}

table tbody tr:nth-child(odd) {
  background: #f8f8f8;
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

.action-column {
  width: 110px;
  text-align: center !important;
}
</style>