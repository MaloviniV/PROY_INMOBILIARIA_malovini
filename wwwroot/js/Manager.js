Vue.createApp({
  data() {
    const app = document.getElementById("app");

    return {
      mensaje: "",
      claseMensaje: "",
      iconoMensaje: "",
      modoLectura: app?.dataset.modo === "lectura",
      mostrarPropietario: app?.dataset.hasPropietario === "true",
      mostrarInquilino: app?.dataset.hasInquilino === "true",
      seccionEditando: "",
      valoresOriginales: {},
      rolActivo:
        app?.dataset.hasPropietario === "true"
          ? "propietario"
          : app?.dataset.hasInquilino === "true"
            ? "inquilino"
            : "",
    };
  },
  methods: {
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
      const formulario = document.getElementById("cliente-manager-form");
      const seccionGuardada = formulario?.querySelector(
        '[name="SeccionGuardada"]',
      );

      if (seccionGuardada) seccionGuardada.value = seccion;
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
