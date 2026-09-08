(() => {
  const root = document.getElementById('inmueble-form');

  if (!root || typeof Vue === 'undefined') {
    return;
  }

  Vue.createApp({
    data() {
      return {
        enviando: false,
        form: {
          direccion: root.querySelector("#Direccion")?.value ?? '',
          imgPortada: root.querySelector("#ImgPortada")?.value ?? '',
          cupo: Number(root.querySelector("#Cupo")?.value ?? 0),
          precio: Number(root.querySelector("#Precio")?.value ?? 0),
          idPropietario: root.querySelector("#IdPropietario")?.value ?? '0',
          idTipo: root.querySelector("#IdTipo")?.value ?? '0',
          estado: root.querySelector("#Estado")?.checked ?? true
        }
      };
    },
    methods: {
      enviarFormulario() {
        this.enviando = true;
      }
    },
    computed: {
      formularioValido() {
        return this.form.direccion.trim().length > 0
          && this.form.precio > 0
          && this.form.idPropietario !== '0'
          && this.form.idTipo !== '0';
      }
    },
    mounted() {
      root.querySelector('form')?.addEventListener('submit', () => {
        this.enviando = true;
      });
    }
  }).mount(root);
})();
