// WindowsProjectSP1.cpp : Defines the entry point for the application.
//

#include "framework.h"
#include "WindowsProjectSP1.h"

#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE hInst;                                // current instance
WCHAR szTitle[MAX_LOADSTRING];                  // The title bar text
WCHAR szWindowClass[MAX_LOADSTRING];            // the main window class name
//2 флага, один разрешает рисование в принципе, второй разрешает измнение координат
//и координаты
//файлы ресурсов уже сгенерированы вижлой для базового приложения, я их только дополнил
//тоесть добавл 2 кнопки в меню с событиями IDM_START и IDM_STOP. События - это громко сказано. Просто число, посылаемое
//в качестве параметра при обработке WM_COMMAND
//использовались функции:
//
//BOOL InvalidateRect(
//	HWND hWnd,           // дескриптор окна
//	CONST RECT* lpRect,  // координаты прямоугольника, если null, то в ес окно
//	BOOL bErase          // состояние очистки, Если этот параметр - TRUE, то фон стирается, когда вызывается функция BeginPaint.
//);
//
//BOOL TextOut(
//	HDC hdc,           // дескриптор DC(Дескриптор контекста устройства.)
//	int nXStart,       // x-координата начальной позиции
//	int nYStart,       // y-координата начальной позиции
//	LPCTSTR lpString,  // строка символов, используем макрос L перед константой, чтобы подходило по формату строки
//	int cbString       // число символов
//
//);
//
//UINT_PTR SetTimer(
//	HWND hWnd,
//	UINT_PTR nIDEvent,		идентификатор таймера отличный от нуля
//	UINT uElapse,			значение времени простоя, в миллисекундах.
//	TIMERPROC lpTimerFunc	 Если параметр lpTimerFunc - NULL, система помещает уведомление WM_TIMER в очередь прикладной программы, а так это указатель на функцию
//);
//в программе каждую секунду таймером кидаю сообщение WM_TIMER


bool draw_flag1 = false;
bool draw_flag2 = false;
int x = 10;
int y = 80;

// Forward declarations of functions included in this code module:
ATOM                MyRegisterClass(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    About(HWND, UINT, WPARAM, LPARAM);

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPWSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    // TODO: Place code here.

    // Initialize global strings
    LoadStringW(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadStringW(hInstance, IDC_WINDOWSPROJECTSP1, szWindowClass, MAX_LOADSTRING);
    MyRegisterClass(hInstance);

    // Perform application initialization:
    if (!InitInstance (hInstance, nCmdShow))
    {
        return FALSE;
    }

    HACCEL hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_WINDOWSPROJECTSP1));

    MSG msg;

    // Main message loop:
    while (GetMessage(&msg, nullptr, 0, 0))
    {
        if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
        {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
    }

    return (int) msg.wParam;
}



//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
    WNDCLASSEXW wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style          = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc    = WndProc;
    wcex.cbClsExtra     = 0;
    wcex.cbWndExtra     = 0;
    wcex.hInstance      = hInstance;
    wcex.hIcon          = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_WINDOWSPROJECTSP1));
    wcex.hCursor        = LoadCursor(nullptr, IDC_ARROW);
    wcex.hbrBackground  = (HBRUSH)(COLOR_WINDOW+1);
    wcex.lpszMenuName   = MAKEINTRESOURCEW(IDC_WINDOWSPROJECTSP1);
    wcex.lpszClassName  = szWindowClass;
    wcex.hIconSm        = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

    return RegisterClassExW(&wcex);
}

//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
   hInst = hInstance; // Store instance handle in our global variable

   HWND hWnd = CreateWindowW(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
      CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, nullptr, nullptr, hInstance, nullptr);

   if (!hWnd)
   {
      return FALSE;
   }

   ShowWindow(hWnd, nCmdShow);
   SetTimer(hWnd, 1, 1000, (TIMERPROC)NULL);//тутачки еще моя строчка:) 
   UpdateWindow(hWnd);

   return TRUE;
}

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE: Processes messages for the main window.
//
//  WM_COMMAND  - process the application menu
//  WM_PAINT    - Paint the main window
//  WM_DESTROY  - post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
    case WM_COMMAND:
        {
            int wmId = LOWORD(wParam);
            // Parse the menu selections:
            switch (wmId)
            {
			case IDM_START:
				draw_flag1 = true;
				draw_flag2 = true;
				InvalidateRect(hWnd, NULL, TRUE);
				break;
			case IDM_STOP:
				draw_flag2 = false;
				break;
            case IDM_ABOUT:
                DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
                break;
            case IDM_EXIT:
                DestroyWindow(hWnd);
                break;
            default:
                return DefWindowProc(hWnd, message, wParam, lParam);
            }
        }
        break;
	case WM_TIMER:
		//HDC h = GetDC(hWnd);
		if (draw_flag2)
		{
			InvalidateRect(hWnd, NULL, TRUE);
			x += 10;
			if (x == 80) x = 10;
		}
		//ReleaseDC(hWnd, h);
		break;
    case WM_PAINT:
        {
			if (draw_flag1) 
			{
				PAINTSTRUCT ps;
				HDC hdc = BeginPaint(hWnd, &ps);
				// TODO: Add any drawing code that uses hdc here...
				TextOut(hdc, x, y, L"lolkekcheburek", 14);
				
				EndPaint(hWnd, &ps);
			}
        }
        break;
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}

// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    UNREFERENCED_PARAMETER(lParam);
    switch (message)
    {
    case WM_INITDIALOG:
        return (INT_PTR)TRUE;

    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
        {
            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
        break;
    }
    return (INT_PTR)FALSE;
}
