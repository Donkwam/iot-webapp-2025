using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebTodoListApp.Models;
using WpfTodoListApp.Models;

namespace WpfTodoListApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        HttpClient client = new HttpClient();
        // ObservableCollection을 굳이 사용하지 않아도 기능에는 문제가 없음
        TodoItemsCollection todoItems = new TodoItemsCollection();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var comboPairs = new List<KeyValuePair<string, int>> {
                new KeyValuePair<string, int>("완료", 1),
                new KeyValuePair<string, int>("미완료", 0),
            };

            CboIsComplete.ItemsSource = comboPairs;

            // RestAPI 호출 준비
            client.BaseAddress = new System.Uri("http://localhost:6200");  // API 서버 URL
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // 데이터 가져오기
            GetDatas();
        }

        private async Task GetDatas()
        {
            // /api/TodoItems GET 메서드 호출
            GrdTodoItems.ItemsSource = todoItems;

            try // API 호출
            {
                // http://localhost:6200/api/TodoItems
                HttpResponseMessage? response = await client.GetAsync("/api/TodoItems");
                response.EnsureSuccessStatusCode(); // 상태코드 확인

                var items = await response.Content.ReadAsAsync<IEnumerable<TodoItem>>();
                todoItems.CopyFrom(items); // ObservableCollection으로 형변환

                // 성공메시지
                await this.ShowMessageAsync("API호출", "로드 성공!");
            }
            catch (Exception ex)
            {
                // 예외메시지
                await this.ShowMessageAsync("API호출 에러", ex.Message);
            }
        }

        // async시 Task가 리턴값이지만 버튼클릭 이벤트메서드와 연결시는 Task -> void로 변경해줘야 연결 가능
        private async void BtnInsert_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var todoItem = new TodoItem
                {
                    Id = 0, // Id는 테이블에서 자동생성 AutoIncrement
                    Title = TxtTitle.Text,
                    TodoDate = Convert.ToDateTime(DtpTodoDate.SelectedDate).ToString("yyyyMMdd"),
                    IsComplete = Convert.ToBoolean(CboIsComplete.SelectedValue)
                };

                // 데이터 입력확인
                //Debug.WriteLine(todoItem.Title);

                // POST 메서드 API 호출
                var response = await client.PostAsJsonAsync("/api/TodoItems", todoItem);
                response.EnsureSuccessStatusCode();

                GetDatas();
            }
            catch (Exception ex)
            {
                // 예외메시지
                await this.ShowMessageAsync("API호출 에러", ex.Message);
            }
        }
    }
}