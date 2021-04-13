﻿// ------------------------------------------ Librerias E Imports ---------------------------------------------------
using System;
using Proyecto2.Misc;

// ------------------------------------------------ Namespace -------------------------------------------------------
namespace Proyecto2.TranslatorAndInterpreter
{
    
    // Clase Principal 
    class PrimitiveValue : AbstractExpression
    {

        // Atributos

        // Type 
        private readonly String StringType;

        // Valor 
        private readonly object Value;

        // Constructor 
        public PrimitiveValue(object Value, String StringType) {

            // Inicicalizar Valores  
            this.Value = Value;
            this.StringType = StringType;
        
        }

        // Métodod Ejecutar 
        public override ObjectReturn Execute(EnviromentTable Env)
        {

            // Objecto A Retornar
            ObjectReturn AuxiliaryReturn;
            
            // Verificar Que Tipo De Valor Primtivo ES 
            if(int.TryParse(this.Value.ToString(), out int AuxiliaryValueI))
            {

                // Agreagr A Objecto Valor 
                AuxiliaryReturn = new ObjectReturn(AuxiliaryValueI, "integer");

            }
            else if(Decimal.TryParse(this.Value.ToString(), out Decimal AuxiliaryValueD))
            {

                // Agregar A Objecto Valor 
                AuxiliaryReturn = new ObjectReturn(AuxiliaryValueD, "real");

            }
            else if(this.Value.ToString() == "true")
            {

                // Agregar A Objecto Valor
                AuxiliaryReturn = new ObjectReturn(true, "boolean");

            }
            else if(this.Value.ToString() == "false")
            {

                // Agregar A Objecto Valor
                AuxiliaryReturn = new ObjectReturn(false, "boolean");

            }
            else
            {

                // Verificar Tipo
                if(this.StringType.Equals("Identifier")) 
                {

                    // Buscar Variable 
                    SymbolTable ActualVar = Env.GetVariable(this.Value.ToString());

                    // Obtener Variable 
                    if(ActualVar != null) 
                    {

                        ObjectReturn ActualValue = (ObjectReturn) ActualVar.Value;

                        // Retornar Objecto 
                        AuxiliaryReturn = new ObjectReturn(ActualValue.Value, ActualVar.Type);

                    }   
                    else
                    {

                        // Retornar Objecto 
                        AuxiliaryReturn = null;

                    }

                }
                else 
                {

                    // Agregar A Objecto Valor
                    AuxiliaryReturn = new ObjectReturn(this.Value.ToString(), "string");

                }

            }

            // Retornar 
            return AuxiliaryReturn;

        }

        // Método Traducir
        public override ObjectReturn Translate(EnviromentTable Env)
        {

            // Verificar Si Es Integer, Real O Boolean
            if (int.TryParse(this.Value.ToString(), out int Value) || Decimal.TryParse(this.Value.ToString(), out Decimal Value_) || this.Value.ToString().ToLower().Equals("true") || this.Value.ToString().ToLower().Equals("false") || this.StringType.Equals("Identifier"))
            {

                // Agregar Traduccion 
                VariablesMethods.TranslateString += this.Value.ToString();

            }
            else 
            {

                // Agregar Traduccion 
                VariablesMethods.TranslateString += "'" + this.Value.ToString() + "'";

            }

            // Retornar Null
            return null;

        }

        // Método Compilar 
        public override ObjectReturn Compilate(EnviromentTable Env) 
        {

            // Objecto A Retornar
            ObjectReturn AuxiliaryReturn;

            // Verificar Que Tipo De Valor Primtivo ES 
            if(int.TryParse(this.Value.ToString(), out int AuxiliaryValueI))
            {

                // Agreagr A Objecto Valor 
                AuxiliaryReturn = new ObjectReturn(AuxiliaryValueI, "integer");

            }
            else if(Decimal.TryParse(this.Value.ToString(), out Decimal AuxiliaryValueD))
            {

                // Agregar A Objecto Valor 
                AuxiliaryReturn = new ObjectReturn(AuxiliaryValueD, "real");

            }
            else if(bool.TryParse(this.Value.ToString(), out bool AuxiliaryValueB))
            {

                // Obtener Instancia 
                ThreeAddressCode Instance_1 = ThreeAddressCode.GetInstance;

                // Agregar Labels 
                if (this.BoolTrue.Equals(""))
                {

                    // Crear Label 
                    this.BoolTrue = Instance_1.CreateLabel();

                }
                if (this.BoolFalse.Equals("")) 
                {

                    // Crear Label 
                    this.BoolFalse = Instance_1.CreateLabel();
                
                }

                // Agregar Comentario 
                Instance_1.AddCommentOneLine("Salto Condición Bool");

                // Verificar Valor 
                if(AuxiliaryValueB)
                {

                    // Agregar Goto 
                    Instance_1.AddNonConditionalJump(this.BoolTrue);

                }
                else
                {

                    // Agregar Goto 
                    Instance_1.AddNonConditionalJump(this.BoolFalse);

                }

                // Pendiente
                AuxiliaryReturn = new ObjectReturn("", "boolean") {

                    BoolTrue = this.BoolTrue,
                    BoolFalse = this.BoolFalse
                
                };

            }
            else
            {

                // Verificar Tipo
                if (this.StringType.Equals("Identifier"))
                {

                    // Buscar Variable 
                    SymbolTable ActualVar = Env.GetVariable(this.Value.ToString());

                    // Obtener Variable 
                    if (ActualVar != null)
                    {

                        ObjectReturn ActualValue = (ObjectReturn) ActualVar.Value;

                        // Retornar Objecto 
                        AuxiliaryReturn = new ObjectReturn(ActualValue.Value, ActualVar.Type);

                    }
                    else
                    {

                        // Retornar Objecto 
                        AuxiliaryReturn = null;

                    }

                }
                else
                {

                    // Obtener Instancia 
                    ThreeAddressCode Instancia_1 = ThreeAddressCode.GetInstance;

                    // Crear Temporal 
                    String ActualTemporary = Instancia_1.CreateTemporary();

                    // Agregar Comentario
                    Instancia_1.AddCommentOneLine("Almacenar String En El Heap (Print)");

                    // Obtener Puntero Heap 
                    Instancia_1.AddOneExpression(ActualTemporary, "HP");

                    // Variable Ascii
                    int Ascii;

                    // Insertar Valores Al Heap 
                    foreach(Char Letter in this.Value.ToString()) 
                    {

                        // Valor Ascii
                        Ascii = (int) Letter;

                        // Agregar Valor A Heap
                        Instancia_1.AddValueToHeap("HP", Ascii.ToString());

                        // Mover Puntero 
                        Instancia_1.MovePointerHeap();

                    }

                    // Agregar Valor A Heap
                    Instancia_1.AddValueToHeap("HP", "-1");

                    // Mover Puntero 
                    Instancia_1.MovePointerHeap();

                    // Agregar A Objecto Valor
                    AuxiliaryReturn = new ObjectReturn(ActualTemporary, "string")
                    {

                        Temporary = true
                    
                    };

                }

            }

            // Retornar 
            return AuxiliaryReturn;

        }

    }

}