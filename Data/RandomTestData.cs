using System;
using System.Collections.Generic;
using testWebAPI.Models.Resources;

namespace testWebAPI.Data
{
    public static class RandomTestData
    {
        public static RandomData[] GetData() => new RandomData[] {
                new RandomData { Id = "86241F4C-C921-421B-859B-2D03D98266D3", Name = "Et Arcu Imperdiet Institute", Rate = 91224 },
                new RandomData{ Id = "76B858EF-0AD2-A824-B02C-95EC3987C813", Name = "Malesuada Malesuada Associates", Rate = 39395 },
                new RandomData{ Id = "733DAD59-7A19-B237-94F3-5086D1FC5AAC", Name = "Eu Odio Phasellus Institute", Rate = 13277 },
                new RandomData{ Id = "C7EEFF13-DA48-CC28-15EC-0ECC13939E36", Name = "Eros Proin Industries", Rate = 58070 },
                new RandomData{ Id = "B65D3A15-89FB-A9E4-EDEB-033381973624", Name = "Dui Nec Associates", Rate = 88310 },
                new RandomData{ Id = "010B90C6-3D7F-3423-0A7D-5CAF8716F1B3", Name = "Duis Industries", Rate = 27432 },
                new RandomData{ Id = "17BA838F-8E3B-329B-02D8-26E152B2E4D7", Name = "Fermentum Limited", Rate = 92722 },
                new RandomData{ Id = "4A894CD9-0C0E-EDDE-8B62-D305A15D2A2C", Name = "Orci Tincidunt Inc.", Rate = 82167 },
                new RandomData{ Id = "3B809B88-9EEE-783E-AF6E-309C99F7C293", Name = "Et Ultrices Industries", Rate = 20390 },
                new RandomData{ Id = "50111157-71FD-96F0-224D-87AFA821F4AC", Name = "Nisi Cum Sociis Institute", Rate = 29685 },
                new RandomData{ Id = "2DEB5851-D70E-5C6E-1606-B150AAFB747A", Name = "Egestas Blandit Nam Company", Rate = 94435 },
                new RandomData{ Id = "BE1B525A-47F8-C0A5-A339-3315E3670D84", Name = "Ultricies LLC", Rate = 59925 },
                new RandomData{ Id = "DC029169-753B-6165-B574-654C11E60734", Name = "Cursus Diam At Incorporated", Rate = 67828 },
                new RandomData{ Id = "0836FAFF-2789-1D2C-43D0-4C4151E0C327", Name = "Id Erat Etiam LLC", Rate = 82513 },
                new RandomData{ Id = "AEB280E4-5882-8393-C23A-4FBC80FC3986", Name = "Magna Institute", Rate = 88758 },
                new RandomData{ Id = "AF73CE9B-F0FA-1156-1BFE-40B530B83AEA", Name = "Eget Company", Rate = 41731 },
                new RandomData{ Id = "69165549-A801-DCEB-4AA4-1FF651D672F8", Name = "Semper Cursus Integer Incorporated", Rate = 51352 },
                new RandomData{ Id = "B57543D4-E69A-133E-34DF-F9B0A2659F53", Name = "Eu Neque PC", Rate = 8906 },
                new RandomData{ Id = "6CD6BA30-1AFD-0426-4176-A711DFCE1C7F", Name = "Arcu Nunc Foundation", Rate = 22236 },
                new RandomData{ Id = "79458D97-D8C8-D020-8B17-DAC16042E430", Name = "Est Ac Facilisis Foundation", Rate = 51918 },
                new RandomData{ Id = "5A500626-DF31-3C65-B5B6-55A925CF58FD", Name = "Aliquam Institute", Rate = 79944 },
                new RandomData{ Id = "21F56383-0A75-F68D-A768-A4F5DFC7BBEF", Name = "Velit LLC", Rate = 38715 }
            };
    }

    public class RandomData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Rate { get; set; }
    }
}
