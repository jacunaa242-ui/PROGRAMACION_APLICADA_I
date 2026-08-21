using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using EjemploMVVM.Commands;
using EjemploMVVM.Models;
using EjemploMVVM.Repositories;

namespace EjemploMVVM.ViewModels
{
    public class ProductoViewModel
    {
        public ObservableCollection<Producto> productos { get; set; }
            = new ObservableCollection<Producto>();

        // NUEVO: categorías
        public ObservableCollection<KeyValuePair<int, string>> categorias { get; set; }
            = new ObservableCollection<KeyValuePair<int, string>>();

        // NUEVO: categoría seleccionada
        public int categoriaSeleccionada { get; set; }

        public RelayCommand ComandoCargarProductos { get; set; }

        public string textoBuscar { get; set; } = string.Empty;

        private ProductoRepositoryImpl _repository;

        public ProductoViewModel()
        {
            _repository = new ProductoRepositoryImpl();

            ComandoCargarProductos =
                new RelayCommand(BuscarProductos);

            CargarCategorias();

            CargarProductos();
        }

        private void BuscarProductos()
        {
            List<Producto> lista;

            // Si seleccionó "Todas"
            if (categoriaSeleccionada == 0)
            {
                lista = _repository.BuscarPorNombre(textoBuscar);
            }
            else
            {
                // Buscar por categoría
                lista = _repository.BuscarPorCategoria(
                    categoriaSeleccionada
                );
            }

            productos.Clear();

            foreach (Producto producto in lista)
            {
                productos.Add(producto);
            }
        }

        public void CargarProductos()
        {
            List<Producto> lista =
                _repository.ListarTodos();

            productos.Clear();

            foreach (Producto producto in lista)
            {
                productos.Add(producto);
            }
        }

        // NUEVO
        private void CargarCategorias()
        {
            categorias.Clear();

            // Primera opción
            categorias.Add(
                new KeyValuePair<int, string>(
                    0,
                    "Todas las categorías"
                )
            );

            Dictionary<int, string> lista =
                _repository.ListarCategorias();

            foreach (var categoria in lista)
            {
                categorias.Add(categoria);
            }
        }
    }
}
