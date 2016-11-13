using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCartoDB.SQL.Linq
{
    /// <summary>
    /// Condiciones lógicas de sql
    /// </summary>
    public enum SqlConditions
    {
        /// <summary>
        /// Se debe cumplir la condición actual y la anterior
        /// </summary>
        AND,
        /// <summary>
        /// Se debe cumplir la condición actual o la anterior
        /// </summary>
        OR,
        /// <summary>
        /// No se debe cumplir la condición actual y cumplirse la anterior
        /// </summary>
        AND_NOT,
        /// <summary>
        /// Se debe cumplir la condición anterior o no cumplirse la actual
        /// </summary>
        OR_NOT,
        /// <summary>
        /// Sin Condición
        /// </summary>
        NONE,
        /// <summary>
        /// Representa el separador en el caso de una sentencia UPDATE
        /// </summary>
        SEPARATOR
    }

    /// <summary>
    /// Tipos de join aceptados
    /// </summary>
    public enum SqlJoin
    {
        /// <summary>
        /// Devuelve todas las filas que cumplan la condición
        /// </summary>
        INNER_JOIN,
        /// <summary>
        /// Devuelve todas las filas de la tabla exterior y las coincidentes en la otra
        /// </summary>
        LEFT_JOIN,
        /// <summary>
        /// Devuelve todas las filas de la tabla interior y las coincidentes en la otra
        /// </summary>
        RIGHT_JOIN,
        /// <summary>
        /// Devolver todas las filas cuando hay una cohincidencia en una de las tablas
        /// </summary>
        FULL_JOIN,
        /// <summary>
        /// Compara todas las columnas entra las dos tablas con el mismo nombre
        /// </summary>
        NATURAL_JOIN,
        /// <summary>
        /// Devuelve el producto cartesiao de ambas tablas
        /// </summary>
        CROSS_JOIN
    }

    /// <summary>
    /// Tipos de orden aplicable
    /// </summary>
    public enum SqlOrder
    {
        /// <summary>
        /// Ascendente (Menor a mayor)
        /// </summary>
        ASC,
        /// <summary>
        /// Descendente (Mayor a menor)
        /// </summary>
        DESC
    }

    /// <summary>
    /// Tipos de querys 
    /// </summary>
    public enum SqlSentence
    {
        /// <summary>
        /// Tipo de consulta sin definir
        /// </summary>
        UNDEFINED,
        /// <summary>
        /// Selecciona registros existentes
        /// </summary>
        SELECT,
        /// <summary>
        /// Inserta registros nuevos
        /// </summary>
        INSERT,
        /// <summary>
        /// Actualiza registros existentes
        /// </summary>
        UPDATE,
        /// <summary>
        /// Elimina registros existentes
        /// </summary>
        DELETE
    }
}
