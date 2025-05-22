
<template>
  <div @click="$emit('click', $event)" class="checkDetailContainer">
    <svg
      class="btn btnCheckDetail"
      :width="size"
      :height="size"
      viewBox="0 0 24 24"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      <path
        d="M13.5 3L19 8.5V20C19 20.55 18.55 21 18 21H6C5.45 21 5 20.55 5 20V4C5 3.45 5.45 3 6 3H13.5Z"
        stroke="#866767"
        stroke-width="2"
        stroke-linecap="round"
        stroke-linejoin="round"
      />
      <path
        d="M14.83 15.83C13.27 17.39 10.73 17.39 9.17 15.83C7.61 14.27 7.61 11.73 9.17 10.17C10.73 8.61 13.27 8.61 14.83 10.17C16.39 11.73 16.39 14.27 14.83 15.83ZM14.83 15.83L19.5 20.5"
        stroke="#866767"
        stroke-width="2"
        stroke-linecap="round"
        stroke-linejoin="round"
      />
    </svg>
  </div>
</template>

<script>
export default {
  name: "SvgCheckDetail",
  props: {
    size: { Number, default: 24 },
  },
};
</script>

<style scoped>
.btnCheckDetail:hover {
  background-color: #fffffffe;
  border-radius: 50%;
  color: #696786;
}

.btnCheckDetail{
  padding: 4px;
}

</style>