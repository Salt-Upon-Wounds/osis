.model tiny
.code
org 100h
.startup
 
call openfilesource  ;открытие файла source.txt
call readfilesource  ;чтение файла   source.txt
call fileclose       ;закрытие файла source.txt  
 
mov bx,offset data
mov di,offset res
 
;mov AX,datalen ;Делимое=10
;mov BL,keylen  ;Делитель=5
;div BL         ;AL=2 (частное)
;mov dl,al
mov dx,datalen/keylen
crypt:mov cx,keylen
      mov si,offset key     ;В SI смещение ключа относительно сегмента данных.
      m1:lodsb              ;Переписывает байт, чей адрес в памяти определяется парой регистров DS:SI, в регистр al. 
         xlatb              ;Посылает в AL элемент массива байтов, у которого адрес нулевого элемента равен ES:DI, а индексом служит старое значение AL. 
         stosb              ;Переписывает содержимое AL в ячейку памяти с адресом ES:DI, после чего прибавляет 1 к DI, если флаг D 0=, и вычитает 1, если D=1.
      loop m1
      
      add bx,keylen
      dec dl
jne crypt
 
    mov ah,9
    mov dx,offset data
    int 21h           ;выводим на экран исходный текст  
    mov dx,offset res      
    int 21h           ;выводим на экран результат 
 
    call createfileencode  ;создание файла encoded  
   ; call readfileencode    ;открытие encoded для записи в него     
    call writefileencode   ;запись в encoded
    call fileclose         ; закрытие файла
 
    mov ah,4ch
    int 21h 
    xor ax,ax
    int 16h
    ret
    
;открытие файла source.txt
openfilesource proc near
     mov ah,3dh         
     mov al,0
     lea dx,source
     int 21h
     mov handle,ax      ;хэндл файла source.txt
     ret
openfilesource endp 
 
;чтение файла source
readfilesource proc near
     mov ah,3fh         
     mov bx,handle      ;хэндл файла 
     lea dx,data       ;читаем сюда
     xor cx,cx          ;cx=0
     mov cl,0Ah         ;столько символов, пусть будет 10
     int 21h
     ret
readfilesource endp     
 
;закрытие любого файла  
fileclose proc near
    mov ah,3eh    
    mov bx,handle
    int 21h
    ret
fileclose endp  
 
;создание файла encoded
createfileencode  proc near
     mov ah,3ch     
     mov cx,0
     lea dx,encoded 
     int 21h     
     ret
createfileencode endp
 
;открытие encoded для записи в него
readfileencode proc near
     mov ah,3dh     
     mov al,1
     lea dx,encoded    
     int 21h         
     mov handle,ax  
     ret
readfileencode endp  
 
;запись в encoded
writefileencode proc near
     mov ah,40h    
     mov bx,handle
     lea dx,res     ;результата
     mov cx,0Ah ;пусть будет 10
     int 21h    
     ret
writefileencode endp     
 
key  db 'kotelnikov'       ;ключ
keylen=$-key             ;длина ключа
;datalen dw ?
data db 10 dup (' ')    ;данные для шифровки
datalen=$-data           ;длина данных
     db 10,13,'$'        ;это для правильного вывода на экран
res  db datalen dup(0)   ;результат
     db 10,13,'$'        ;это для правильного вывода на экран
handle dw ?              ;хэндл файла 
encoded db 'encoded.txt',0  ;из него будет браться исходное сообщение
source  db 'source.txt',0   ;в него будет записано шифрованыый сообщение
end