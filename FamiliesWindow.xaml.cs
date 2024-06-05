namespace KIPRO.BIM.RevitPlugin
{
    using Autodesk.Revit.UI;
    using System.Windows;
    using System.Windows.Controls;

    public partial class FamiliesWindow : Window
    {
        public FamiliesWindow()
        {
            InitializeComponent();

            PopulateTreeView();
        }

        private void PlaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (EquipmentTreeView.SelectedItem != null)
            {
                // TODO Логика для размещения в модели

                var selectedItem = EquipmentTreeView.SelectedItem as TreeViewItem;
                TaskDialog.Show("Окно выбора семейств", $"Размещаем: {selectedItem.Header}");
                this.Close();
            }
            else
            {
                TaskDialog.Show("Окно выбора семейств", "Выберите элемент для размещения.");
                this.Close();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO Логика для обновления всех семейств
            /**/ TaskDialog.Show("Окно выбора семейств", "Обновляем все семейства.");
        }

        #region Заготовка дерева по умолчанию

        private void PopulateTreeView()
        {
            // Автоматизация инженерных систем
            var autoSystem = new TreeViewItem { Header = "Автоматизация инженерных систем" };
            autoSystem.Items.Add(CreateTreeViewItem("ABB", new[] { "Компактный коммутатор 8 x 10/100BaseTX EDS-208A", "Контроллер PM5630" }));
            autoSystem.Items.Add(CreateTreeViewItem("IECON", new[] { "Реле времени IECON-RT01", "Программируемый логический контроллер IECON-PLC20" }));
            autoSystem.Items.Add(CreateTreeViewItem("Moxa", new[] { "Коммутатор Moxa EDS-205A", "Преобразователь MOXA NPort 5150A" }));
            autoSystem.Items.Add(CreateTreeViewItem("Siemens", new[] { "ПЛК Siemens S7-1200", "Частотный преобразователь Siemens Sinamics V20" }));
            EquipmentTreeView.Items.Add(autoSystem);

            // Видеонаблюдение
            var videoSurveillance = new TreeViewItem { Header = "Видеонаблюдение" };
            videoSurveillance.Items.Add(CreateTreeViewItem("Камеры видеонаблюдения", new[] { "IP-камера Hikvision DS-2CD2143G0-I", "Аналоговая камера Dahua HAC-HDW1200TLP" }));
            videoSurveillance.Items.Add(CreateTreeViewItem("Видеорегистраторы", new[] { "NVR Hikvision DS-7608NI-K2", "DVR Dahua XVR5108HS-X" }));
            EquipmentTreeView.Items.Add(videoSurveillance);

            // Газопроводы
            var gasPipelines = new TreeViewItem { Header = "Газопроводы" };
            gasPipelines.Items.Add(CreateTreeViewItem("ПГБ-50", new[] { "ПГБ-50 Вводной", "ПГБ-50 Распределительный" }));
            gasPipelines.Items.Add(CreateTreeViewItem("ГРПШ-10", new[] { "ГРПШ-10 Вводной", "ГРПШ-10 Распределительный" }));
            EquipmentTreeView.Items.Add(gasPipelines);

            // Диспетчеризация лифтов
            var liftDispatch = new TreeViewItem { Header = "Диспетчеризация лифтов" };
            liftDispatch.Items.Add(new TreeViewItem { Header = "Система диспетчеризации OTIS" });
            liftDispatch.Items.Add(new TreeViewItem { Header = "Система диспетчеризации KONE" });
            EquipmentTreeView.Items.Add(liftDispatch);

            // Домофония
            var intercom = new TreeViewItem { Header = "Домофония" };
            intercom.Items.Add(CreateTreeViewItem("Видеодомофоны", new[] { "Видеодомофон Commax CDV-70UM", "Видеодомофон Kocom KCV-A374SD" }));
            intercom.Items.Add(CreateTreeViewItem("Аудиодомофоны", new[] { "Аудиодомофон Commax DP-SS", "Аудиодомофон Kocom KDP-601A" }));
            EquipmentTreeView.Items.Add(intercom);

            // ОЗДС
            var ozds = new TreeViewItem { Header = "ОЗДС" };
            ozds.Items.Add(CreateTreeViewItem("Система оповещения", new[] { "Система оповещения BOSCH", "Система оповещения TOA" }));
            ozds.Items.Add(CreateTreeViewItem("Система оповещения пожарной безопасности", new[] { "Система ПБ BOSCH", "Система ПБ TOA" }));
            EquipmentTreeView.Items.Add(ozds);

            // Охранная сигнализация
            var securitySystem = new TreeViewItem { Header = "Охранная сигнализация" };
            securitySystem.Items.Add(new TreeViewItem { Header = "Пульт охранной сигнализации AJAX Hub" });
            securitySystem.Items.Add(new TreeViewItem { Header = "Датчик движения AJAX MotionProtect" });
            securitySystem.Items.Add(new TreeViewItem { Header = "Датчик открытия AJAX DoorProtect" });
            EquipmentTreeView.Items.Add(securitySystem);

            // Радиофикация
            var radiofication = new TreeViewItem { Header = "Радиофикация" };
            radiofication.Items.Add(CreateTreeViewItem("Усилители", new[] { "Усилитель мощности BOSCH PLE-1P120-EU", "Усилитель мощности TOA A-2240" }));
            radiofication.Items.Add(CreateTreeViewItem("Радиоприемники", new[] { "Радиоприемник Philips OR7200", "Радиоприемник Sony XDR-S61D" }));
            EquipmentTreeView.Items.Add(radiofication);

            // Сети связи
            var communicationNetworks = new TreeViewItem { Header = "Сети связи" };
            communicationNetworks.Items.Add(CreateTreeViewItem("Коммутаторы", new[] { "Коммутатор Cisco SG250-26", "Коммутатор TP-Link TL-SG1024" }));
            communicationNetworks.Items.Add(CreateTreeViewItem("Маршрутизаторы", new[] { "Маршрутизатор MikroTik hAP ac2", "Маршрутизатор Ubiquiti EdgeRouter X" }));
            EquipmentTreeView.Items.Add(communicationNetworks);

            // СКУД
            var scud = new TreeViewItem { Header = "СКУД" };
            scud.Items.Add(CreateTreeViewItem("Контроллеры доступа", new[] { "Контроллер доступа ZKTeco InBio460", "Контроллер доступа PERCo-CT/L04.3" }));
            scud.Items.Add(CreateTreeViewItem("Считыватели", new[] { "Считыватель карт HID iCLASS SE", "Считыватель карт ZKTeco ProID30" }));
            EquipmentTreeView.Items.Add(scud);

            // Без привязки к производителю
            var noManufacturer = new TreeViewItem { Header = "Без привязки к производителю" };
            noManufacturer.Items.Add(new TreeViewItem { Header = "Электрооборудование" });
            noManufacturer.Items.Add(new TreeViewItem { Header = "Слаботочные системы" });
            EquipmentTreeView.Items.Add(noManufacturer);

            // УГО
            var ugo = new TreeViewItem { Header = "УГО" };
            ugo.Items.Add(new TreeViewItem { Header = "Устройство" });
            ugo.Items.Add(new TreeViewItem { Header = "Группа объектов" });
            EquipmentTreeView.Items.Add(ugo);

            // Марки
            var marks = new TreeViewItem { Header = "Марки" };
            marks.Items.Add(new TreeViewItem { Header = "Электрика" });
            marks.Items.Add(new TreeViewItem { Header = "Отопление" });
            EquipmentTreeView.Items.Add(marks);
        }

        private TreeViewItem CreateTreeViewItem(string header, string[] subItems)
        {
            var item = new TreeViewItem { Header = header };
            foreach (var subItem in subItems)
            {
                item.Items.Add(new TreeViewItem { Header = subItem });
            }
            return item;
        }

        #endregion
    }
}
