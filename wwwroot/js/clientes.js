Vue.createApp({
  data() {
    return {
      filtroActual: "TODOS",
      dniBusqueda: "",
      mensajeValidacion: "",
      claseAlerta: "",
      clienteEncontrado: false,
      dniVerificado: false,
      clientes: [
        {
          dni: "11111111",
          nombre: "Juan",
          apellido: "Pérez",
          esPropietario: true,
          esInquilino: false,
        },
        {
          dni: "22222222",
          nombre: "María",
          apellido: "Gómez",
          esPropietario: false,
          esInquilino: true,
        },
        {
          dni: "33333333",
          nombre: "Carlos",
          apellido: "López",
          esPropietario: true,
          esInquilino: true,
        },
      ],
    };
  },
  computed: {
    dniValido() {
      return this.dniBusqueda.length >= 7;
    },
    classActiveTodos() {
      return { active: this.filtroActual === "TODOS" };
    },
    classActivePropietarios() {
      return { active: this.filtroActual === "PROPIETARIOS" };
    },
    classActiveInquilinos() {
      return { active: this.filtroActual === "INQUILINOS" };
    },
    clientesFiltrados() {
      if (this.filtroActual === "TODOS") {
        return this.clientes;
      } else if (this.filtroActual === "PROPIETARIOS") {
        return this.clientes.filter((c) => c.esPropietario);
      } else if (this.filtroActual === "INQUILINOS") {
        return this.clientes.filter((c) => c.esInquilino);
      }
    },
  },
  methods: {
    normalizarDni(event) {
      this.dniBusqueda = event.target.value.replace(/\D/g, "").slice(0, 8);
      this.clienteEncontrado = false;
      this.dniVerificado = false;
      this.mensajeValidacion = "";
    },
    limpiarDni() {
      this.dniBusqueda = "";
      this.clienteEncontrado = false;
      this.dniVerificado = false;
      this.mensajeValidacion = "";
      this.claseAlerta = "";
    },
    async verificarDni() {
      try {
        //Espero la variable "exite" y "nombreCompleto"
        const response = await fetch(
          `/Clientes/verificarDni?dni=${this.dniBusqueda}`,
        );
        const data = await response.json();

        this.clienteEncontrado = data.existe;
        this.dniVerificado = true;

        if (this.clienteEncontrado) {
          this.claseAlerta = "alert-warning";
          this.mensajeValidacion = `El DNI ya pertenece a ${data.nombreCompleto}. ¿Deseas agregarle un nuevo rol?`;
        } else {
          this.claseAlerta = "alert-success";
          this.mensajeValidacion =
            "DNI libre. Puedes registrar al nuevo cliente.";
        }
      } catch (error) {
        console.error("Error al buscar en el servidor");
      }
    },
  },
}).mount("#app");
