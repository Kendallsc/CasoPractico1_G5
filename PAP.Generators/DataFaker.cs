using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PAP.Generators
{
    /// <summary>
    /// Generador de datos aleatorios "realistas" sin librerias externas.
    /// Combina listas fijas (nombres, ciudades, palabras) con Random para producir los valores.
    /// </summary>
    public class DataFaker
    {
        private readonly Random _random;

        public static readonly string[] FirstNames =
        {
            "Sofia", "Valentina", "Camila", "Isabella", "Mariana", "Lucia", "Daniela", "Gabriela", "Andrea", "Fernanda",
            "Paula", "Natalia", "Carolina", "Laura", "Elena", "Ana", "Maria", "Raquel", "Jimena", "Adriana",
            "Mateo", "Santiago", "Sebastian", "Diego", "Nicolas", "Alejandro", "Daniel", "Gabriel", "Samuel", "David",
            "Andres", "Javier", "Carlos", "Luis", "Jorge", "Ricardo", "Fernando", "Esteban", "Pablo", "Kendall"
        };

        public static readonly string[] LastNames =
        {
            "Rodriguez", "Gonzalez", "Hernandez", "Jimenez", "Vargas", "Rojas", "Mora", "Araya", "Solano", "Castro",
            "Alvarado", "Chaves", "Ramirez", "Sanchez", "Quesada", "Salazar", "Calderon", "Arias", "Villalobos", "Campos",
            "Gutierrez", "Mendez", "Navarro", "Brenes", "Cordero", "Herrera", "Murillo", "Soto", "Valverde", "Zuniga"
        };

        public static readonly string[] Countries =
        {
            "Costa Rica", "Mexico", "Colombia", "Argentina", "Chile", "Espana", "Peru", "Uruguay", "Panama", "Guatemala"
        };

        /// <summary>
        /// Ciudad y provincia/estado de Costa Rica (las direcciones se generan en CR).
        /// </summary>
        public static readonly string[][] CostaRicaCities =
        {
            new[] { "San Jose", "San Jose" }, new[] { "Escazu", "San Jose" }, new[] { "Desamparados", "San Jose" },
            new[] { "Curridabat", "San Jose" }, new[] { "Alajuela", "Alajuela" }, new[] { "Grecia", "Alajuela" },
            new[] { "San Ramon", "Alajuela" }, new[] { "Cartago", "Cartago" }, new[] { "Paraiso", "Cartago" },
            new[] { "Heredia", "Heredia" }, new[] { "Santo Domingo", "Heredia" }, new[] { "Liberia", "Guanacaste" },
            new[] { "Nicoya", "Guanacaste" }, new[] { "Puntarenas", "Puntarenas" }, new[] { "Limon", "Limon" }
        };

        public static readonly string[] Streets =
        {
            "Avenida Central", "Calle 5", "Avenida 2", "Calle Los Almendros", "Barrio Escalante", "Calle Real",
            "Avenida Segunda", "Calle El Carmen", "Residencial Las Flores", "Calle La Paz", "Barrio Amon", "Paseo Colon"
        };

        private static readonly string[] TitleNouns =
        {
            "jardin", "silencio", "sombra", "viaje", "laberinto", "rio", "bosque", "invierno", "ciudad", "secreto",
            "espejo", "camino", "faro", "puerto", "reino", "destino", "tiempo", "memoria", "horizonte", "volcan"
        };

        private static readonly string[] TitleAdjectives =
        {
            "perdido", "eterno", "olvidado", "oscuro", "dorado", "infinito", "escondido", "ultimo", "salvaje", "dormido",
            "lejano", "prohibido", "silencioso", "antiguo", "sagrado"
        };

        private static readonly string[] TitleComplements =
        {
            "de la noche", "del mar", "de los suenos", "de cristal", "de la montana", "del norte", "de las estrellas",
            "de la lluvia", "del olvido", "de papel", "del alba", "de los sabios"
        };

        private static readonly string[] Words =
        {
            "historia", "aventura", "familia", "misterio", "amor", "guerra", "ciencia", "viaje", "destino", "verdad",
            "personaje", "mundo", "pueblo", "descubre", "enfrenta", "recorre", "revela", "transforma", "narra", "explora",
            "intensa", "inolvidable", "profunda", "sorprendente", "emotiva", "clasica", "moderna", "unica", "valiente", "humana"
        };

        public static readonly string[] Languages = { "Espanol", "Ingles", "Frances", "Portugues", "Italiano", "Aleman" };

        public DataFaker()
            : this(Environment.TickCount)
        {
        }

        public DataFaker(int seed)
        {
            _random = new Random(seed);
        }

        public int Int(int min, int maxInclusive)
        {
            return _random.Next(min, maxInclusive + 1);
        }

        public bool Chance(int percent)
        {
            return _random.Next(100) < percent;
        }

        public T Pick<T>(IList<T> items)
        {
            return items[_random.Next(items.Count)];
        }

        /// <summary>
        /// Toma "count" elementos distintos de la lista (sin repetir).
        /// </summary>
        public List<T> PickMany<T>(IList<T> items, int count)
        {
            return items.OrderBy(x => _random.Next()).Take(Math.Min(count, items.Count)).ToList();
        }

        public decimal Money(decimal min, decimal max)
        {
            decimal value = min + (decimal)_random.NextDouble() * (max - min);
            return Math.Round(value, 2);
        }

        public DateTime Date(DateTime from, DateTime to)
        {
            int range = Math.Max(1, (int)(to - from).TotalDays);
            return from.Date.AddDays(_random.Next(range));
        }

        public DateTime DateTimeBetween(DateTime from, DateTime to)
        {
            long ticks = (long)(_random.NextDouble() * (to - from).Ticks);
            return from.AddTicks(ticks);
        }

        public DateTime BirthDate(int minAge, int maxAge)
        {
            DateTime today = DateTime.Today;
            return Date(today.AddYears(-maxAge), today.AddYears(-minAge));
        }

        public string FirstName()
        {
            return Pick(FirstNames);
        }

        public string LastName()
        {
            return Pick(LastNames);
        }

        public string Country()
        {
            return Pick(Countries);
        }

        /// <summary>
        /// Telefono con formato de Costa Rica: +506 8xxx-xxxx
        /// </summary>
        public string Phone()
        {
            return string.Format("+506 {0}{1:000}-{2:0000}", Pick(new[] { 2, 6, 7, 8 }), Int(0, 999), Int(0, 9999));
        }

        /// <summary>
        /// Correo unico: el indice garantiza que no se repita (las columnas Email tienen restriccion UNIQUE).
        /// </summary>
        public string Email(string firstName, string lastName, int index, string domain)
        {
            return string.Format("{0}.{1}{2}@{3}", Slug(firstName), Slug(lastName), index, domain);
        }

        public string Sentence(int minWords, int maxWords)
        {
            int count = Int(minWords, maxWords);
            var words = new List<string>();
            for (int i = 0; i < count; i++)
                words.Add(Pick(Words));

            string text = string.Join(" ", words);
            return char.ToUpper(text[0]) + text.Substring(1) + ".";
        }

        public string Paragraph(int sentences)
        {
            var parts = new List<string>();
            for (int i = 0; i < sentences; i++)
                parts.Add(Sentence(6, 12));
            return string.Join(" ", parts);
        }

        public string BookTitle()
        {
            string noun = Pick(TitleNouns);
            switch (_random.Next(3))
            {
                case 0:
                    return Capitalize("el " + noun + " " + Pick(TitleAdjectives));
                case 1:
                    return Capitalize("el " + noun + " " + Pick(TitleComplements));
                default:
                    return Capitalize("cronicas del " + noun + " " + Pick(TitleAdjectives));
            }
        }

        /// <summary>
        /// ISBN-13 valido (prefijo 978 + digito verificador). El indice lo hace unico.
        /// </summary>
        public string Isbn13(int index)
        {
            string body = "978" + (100000000 + index).ToString("000000000", CultureInfo.InvariantCulture);
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = body[i] - '0';
                sum += i % 2 == 0 ? digit : digit * 3;
            }
            int check = (10 - sum % 10) % 10;
            return body + check;
        }

        public string PostalCode()
        {
            return Int(10101, 70703).ToString(CultureInfo.InvariantCulture);
        }

        public string Reference(string prefix)
        {
            return prefix + "-" + Guid.NewGuid().ToString("N").Substring(0, 12).ToUpperInvariant();
        }

        /// <summary>
        /// Texto en minusculas sin espacios ni tildes, util para correos y URLs.
        /// </summary>
        public static string Slug(string text)
        {
            string normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in normalized)
            {
                if (char.IsLetterOrDigit(c) && CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(char.ToLowerInvariant(c));
            }
            return sb.ToString();
        }

        private static string Capitalize(string text)
        {
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}
