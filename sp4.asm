model tiny
code    segment
	org 100h
	assume  cs:code,ds:code,es:code
	
start:
jmp NotResPart
NewProc:
cmp ah,9
je OK
jmp dword ptr[cs:OldVect]  ; а вот здесь пришлось добавить PTR, иначе не пахало
OK:
push ds
push dx
;--------
push ax
push bx
push cx
push si
push di
pushf
;-------------------------------------------
mov si,dx
mov di,si

mov cx,0

l1:
mov al,[si]
inc si
cmp al,' '
je l1;Переход, если равно
new_word:
mov [di],al
inc di
l2: ;проигнорили начальные пробелы
mov al,[si]
inc si
cmp al,' '
je l3;встретили снова пробелы после слова
cmp al,'$'
je space_exit;встретили конец без пробела
mov [di],al
inc di
jmp l2; скопировали слово
l3:;встретили пробел
inc cx
mov [di],al
inc di
l4:
mov al,[si]
inc si
cmp al,'$'
je exit;встретили конец
cmp al,' '
jne new_word;началось новое слово
jmp l4;игнорим пробелы

space_exit:
mov [di],' '
mov [di+1],al
jmp real_exit

exit:
mov [di],al

real_exit:
;------------------------------
;в cx число пробелов=число слов
;Процедура преобразования слова в строку в десятичном виде (без знака)
; AX - слово
; DI - буфер для строки (5 символов). Значение регистра не сохраняется.
mov ax,cx
mov di,dx

word_to_udec_str:
    push ax
    push cx
    push dx
    push bx
    xor cx,cx               ;Обнуление CX
    mov bx,10               ;В BX делитель (10 для десятичной системы)
 
wtuds_lp1:                  ;Цикл получения остатков от деления
    xor dx,dx               ;Обнуление старшей части двойного слова
    div bx                  ;Деление AX=(DX:AX)/BX, остаток в DX
    add dl,'0'              ;Преобразование остатка в код символа
    push dx                 ;Сохранение в стеке
    inc cx                  ;Увеличение счетчика символов
    test ax,ax              ;Проверка AX
    jnz wtuds_lp1           ;Переход к началу цикла, если частное не 0.
 
wtuds_lp2:                  ;Цикл извлечения символов из стека
    pop dx                  ;Восстановление символа из стека
    mov [di],dl             ;Сохранение символа в буфере
    inc di                  ;Инкремент адреса буфера
    loop wtuds_lp2          ;Команда цикла
 
	mov [di],'$'

    pop bx
    pop dx
    pop cx
    pop ax

last_exit:
;-----------------------------------
popf
pop di
pop si
pop cx
pop bx
pop ax

;---------------------------
;mov dx,offset @Str
pushf
call dword ptr[cs:OldVect] 
pop dx
pop ds
iret
num dw 0
@Str db 'MyString$'
OldVect dd ?
	
NotResPart:
mov ax,3521h
int 21h
mov word ptr [OldVect], bx
mov word ptr [OldVect+2], es
	
mov ax,2521h
mov dx,offset NewProc
int 21h
	
mov dx,offset NotResPart
int 27h
ends code
end start