// AVS2.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <Windows.h>

using namespace std;

void GetFileClusters(PCTSTR lpFileName) 
{
	HANDLE hFile;
	ULONG OutSize, Bytes, CnCount, r, ClusterSize, FileSize, ClCount;
	BOOLEAN Result = FALSE;
	LARGE_INTEGER PrevVCN, Lcn;
	STARTING_VCN_INPUT_BUFFER InBuf;
	PRETRIEVAL_POINTERS_BUFFER OutBuf;
	hFile = CreateFile(lpFileName, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, 0);

	if (hFile != INVALID_HANDLE_VALUE)
	{
		ULONG SecPerCl, BtPerSec, ClusterSize, ClCount, FileSize, i;
		char DriveName[3] = { 'F', ':', 0 };
		GetDiskFreeSpaceA(DriveName, &SecPerCl, &BtPerSec, NULL, NULL);
		ClusterSize = SecPerCl * BtPerSec;
		char* buff = (char*)malloc(ClusterSize+1);


		FileSize = GetFileSize(hFile, NULL);
		cout << "clsize=" << ClusterSize << endl << "fsize=" << FileSize << endl;

		OutSize = sizeof(RETRIEVAL_POINTERS_BUFFER) + (FileSize / ClusterSize) * sizeof(OutBuf->Extents);
		OutBuf = (PRETRIEVAL_POINTERS_BUFFER)malloc(OutSize);
		InBuf.StartingVcn.QuadPart = 0;
		cout << "File " << (char*)lpFileName << " allocated in clusters:" << endl;
		if (DeviceIoControl(hFile, FSCTL_GET_RETRIEVAL_POINTERS, &InBuf, sizeof(InBuf), OutBuf, OutSize, &Bytes, NULL))
		{
			DWORD readbytes;
			
			ClCount = (FileSize + ClusterSize - 1) / ClusterSize;
			PrevVCN = OutBuf->StartingVcn;
			for (r = 0; r < OutBuf->ExtentCount; r++)
			{
				ReadFile(hFile, buff, ClusterSize, &readbytes, NULL);
				buff[readbytes] = '\0';
				printf("%s\n", buff);

				Lcn = OutBuf->Extents[r].Lcn;
				for (CnCount = OutBuf->Extents[r].NextVcn.QuadPart - PrevVCN.QuadPart; CnCount; CnCount--, Lcn.QuadPart++)
				{
					cout << Lcn.QuadPart << endl;
				}

				PrevVCN = OutBuf->Extents[r].NextVcn;
			}
		}

		
		free(buff);
		free(OutBuf);
		CloseHandle(hFile);
	}
}

int main()
{
	GetFileClusters(L"F:\\first.txt");
}

// Запуск программы: CTRL+F5 или меню "Отладка" > "Запуск без отладки"
// Отладка программы: F5 или меню "Отладка" > "Запустить отладку"

// Советы по началу работы 
//   1. В окне обозревателя решений можно добавлять файлы и управлять ими.
//   2. В окне Team Explorer можно подключиться к системе управления версиями.
//   3. В окне "Выходные данные" можно просматривать выходные данные сборки и другие сообщения.
//   4. В окне "Список ошибок" можно просматривать ошибки.
//   5. Последовательно выберите пункты меню "Проект" > "Добавить новый элемент", чтобы создать файлы кода, или "Проект" > "Добавить существующий элемент", чтобы добавить в проект существующие файлы кода.
//   6. Чтобы снова открыть этот проект позже, выберите пункты меню "Файл" > "Открыть" > "Проект" и выберите SLN-файл.
