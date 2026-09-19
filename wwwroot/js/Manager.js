Vue.createApp({
  data() {
    const app = document.getElementById("app");

    return {
      modoLectura: app?.dataset.modo === "lectura",
      seccionEditando: "",
      rolActivo: "propietario",

      tienePropietario: app?.dataset.tienePropietario === "True",
      tieneInquilino: app?.dataset.tieneInquilino === "True"
    };
  },
  methods: {
    editando(seccion) {
      return !(this.modoLectura || this.seccionEditando !== seccion);
    },
    classActive(rol) {
      return { active: this.rolActivo === rol };
    },
    iniciarEdicion(seccion) {
      if (this.seccionEditando && this.seccionEditando !== seccion) {
        this.cancelarEdicion(this.seccionEditando);
      }

      this.seccionEditando = seccion;
    },
    guardarSeccion(seccion) {
      const formulario = this.$refs[`form-${seccion}`];

      if (formulario) {
        formulario.requestSubmit();
        return;
      }

      this.seccionEditando = "";
    },
    cancelarEdicion(seccion) {
      const formulario = this.$refs[`form-${seccion}`];

      formulario?.reset();

      this.seccionEditando = "";
    },
  },
}).mount("#app");
