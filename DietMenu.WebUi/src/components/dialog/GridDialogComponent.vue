<template>
  <div class="modal-body">
    <GridComponent
      :canAdd="false"
      :canDelete="false"
      :canEdit="false"
      :canFilter="true"
      :canMultiSelect="options.canMultiSelect"
      :canPage="true"
      :canSelect="true"
      :canSort="true"
      :columns="options.columns"
      :getGridData="options.getGridData"
      :initialFilter="options.initialFilter"
      ref="grid"
    />
  </div>
</template>

<script lang="ts">
import GridComponent from "@/components/grid/GridComponent.vue";
import { ref, Ref } from "vue";

export default {
  components: {
    GridComponent,
  },
  props: ["options"],
  setup(props: any) {
    const grid: Ref<any> = ref(null);

    const select = () => {
      const data = grid.value?.selected;

      if (data?.lenght > 0) {
        props.options.selectAction(data);
      }
    };

    return {
      grid,
      select,
    };
  },
};
</script>