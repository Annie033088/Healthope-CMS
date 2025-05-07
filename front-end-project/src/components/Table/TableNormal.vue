<template>
  <div class="table-container">
    <table>
      <thead>
        <tr>
          <th>Name</th>
          <th>DoB</th>
          <th>Role</th>
          <th>Salary</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td>Nandor the Relentless</td>
          <td>04/06/1262</td>
          <td>Vampire Warrior</td>
          <td>$0</td>
        </tr>
        <tr>
          <td>Laszlo Cravensworth</td>
          <td>19/10/1678</td>
          <td>Vampire Gentleman</td>
          <td>$0</td>
        </tr>
        <tr>
          <td>Nadja</td>
          <td>15/03/1593</td>
          <td>Vampire Seductress</td>
          <td>$0</td>
        </tr>
        <tr>
          <td>Colin Robinson</td>
          <td>01/09/1971</td>
          <td>Energy Vampire</td>
          <td>$53,000</td>
        </tr>
        <tr>
          <td>Guillermo de la Cruz</td>
          <td>18/11/1991</td>
          <td>Familiar/Vampire Hunter</td>
          <td>$0</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
      name: "TableNormal",
}
</script>

<style scoped>
.table-container {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
}

thead {
  text-align: left;
}

th,
td {
  padding: 0.5rem 0.75rem;
  white-space: nowrap;
}

th {
  font-weight: 500;
  color: #1f2937; /* gray-900 */
  border-bottom: 2px solid #e5e7eb; /* divide-gray-200 */
}

tbody tr {
  border-bottom: 1px solid #e5e7eb; /* divide-gray-200 */
}

tbody tr:nth-child(even) {
  background-color: #f9fafb; /* gray-50 */
}

tbody td {
  color: #1f2937; /* gray-900 */
}

tbody td:first-child {
  font-weight: 500;
}
</style>