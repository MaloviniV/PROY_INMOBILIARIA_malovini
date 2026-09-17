Vue.createApp({
  data() {
    const app = document.getElementById("app");

    return {
      modo: app?.dataset.modo || "crear",
      mensaje: "",
      claseMensaje: "",
      iconoMensaje: "",
      modoLectura: this.modo === "lectura",
      seccionEditando: "",
      valoresOriginales: {},
      rolActivo: "propietario"
    };
  },
  methods: {
    classActive(rol) {
      return { active: this.rolActivo === rol };
    },
    iniciarEdicion(seccion) {
      if (this.modoLectura) {
        const dni = document.querySelector('[name="Persona.Dni"]')?.value;
        if (dni) {
          window.location.href = `/Clientes/Manager?modo=edicion&dni=${encodeURIComponent(dni)}`;
        }
        return;
      }

      const elemento = document.querySelector(`[data-seccion="${seccion}"]`);

      if (!elemento) return;

      this.valoresOriginales[seccion] = Array.from(
        elemento.querySelectorAll("input, textarea, select"),
      )
        .filter((campo) => campo.name)
        .map((campo) => ({ name: campo.name, value: campo.value }));
      this.seccionEditando = seccion;
    },
    guardarSeccion(seccion) {
      const formulario = document.querySelector(
        `form[data-seccion="${seccion}"]`,
      );

      if (formulario) {
        formulario.requestSubmit();
        return;
      }

      this.seccionEditando = "";
      delete this.valoresOriginales[seccion];
    },
    cancelarEdicion(seccion) {
      const elemento = document.querySelector(`[data-seccion="${seccion}"]`);
      const valores = this.valoresOriginales[seccion] || [];

      valores.forEach(({ name, value }) => {
        const campo = elemento?.querySelector(`[name="${name}"]`);
        if (campo) campo.value = value;
      });

      this.seccionEditando = "";
      delete this.valoresOriginales[seccion];
    },
  },
}).mount("#app");
