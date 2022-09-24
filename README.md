## Implementação do analisador sintático ascendente SLR

Aceita uma gramática para os elementos definidos abaixo e implementa o analisador sintático ascendente SLR para esta gramática:
```
Função
Lista de parâmetros
Declaração de variável
Comandos de atribuição, if, while, print e chamada de função
```
Exemplo de código fonte de entrada:
```
function x (int a, int b){
   x = a + b;
   if (x > 10){
      print(x);

   }

   int i;
   while(i < 16) {
      t = a / b;
      i = i + 1;
   }

}
```

### Arquivos relacionados:
- [Documentação (Gramática, estados, reduções e tabela)](docs)
- [Formulário principal](Compilador/FormMain.cs)
- [Diretório analizador léxico](Compilador/Lexico)
- [Diretório analisador sintático](Compilador/Sintatico/Ascendente/SLR)
- [Arquivo executável](Compilador/bin/Debug/Compilador.exe)
