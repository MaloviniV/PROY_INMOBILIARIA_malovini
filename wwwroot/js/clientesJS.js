Vue.createApp({
  data() {
    return {
      dniBusqueda: "",
      dniVerificado: false,
      clienteEncontrado: false,
      mensajeValidacion: "",
      claseAlerta: "",

      filtroActual: "TODOS",
      clientes: [],
    };
  },
  computed: {
    dniValido() {
      return this.dniBusqueda.length >= 7;
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
  async mounted() {
    try {
      const response = await fetch("/Clientes/ListarClientes");
      const result = await response.json();

      if (!response.ok || !result.success) {
        throw new Error(
          result.message || "No se pudo cargar el listado de clientes.",
        );
      }

      this.clientes = result.data;
    } catch (error) {
      console.error("Error al cargar clientes:", error);
      this.mensajeValidacion = error.message;
      this.claseAlerta = "alert-danger";
    }
  },
  methods: {
    classActive(filtro) {
      return { active: this.filtroActual === filtro };
    },
    filtrarInputDni(event) {
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
    async buscarDni() {
      try {
        //Espero la variable "data", "nombre" y "apellido"
        const response = await fetch(
          `/Clientes/BuscarPorDni?dni=${encodeURIComponent(this.dniBusqueda)}`,
        );
        const data = await response.json();

        if (!response.ok) {
          this.dniVerificado = false;
          this.claseAlerta = "alert-danger";
          this.mensajeValidacion =
            data.message || "El DNI ingresado no es válido.";
          return;
        }

        this.clienteEncontrado = data.success;
        this.dniVerificado = true;

        if (this.clienteEncontrado) {
          this.claseAlerta = "alert-warning";
          this.mensajeValidacion = `El DNI ya pertenece a ${data.data.apellido} ${data.data.nombre}. ¿Deseas agregarle un nuevo rol?`;
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
