using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GAT_Produkcja.GUSServiceReference;

namespace GAT_Produkcja.Utilities.GUS
{
    class GUSApi
    {
        public void PolaczBIRpub()
        {

            //Create the binding.
            var myBinding = new WSHttpBinding(SecurityMode.Transport);
            myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            myBinding.MessageEncoding = WSMessageEncoding.Mtom;

            var ea = new EndpointAddress("https://wyszukiwarkaregontest.stat.gov.pl/wsBIR/UslugaBIRzewnPubl.svc"); //test
            var client = new UslugaBIRzewnPublClient(myBinding, ea);
                client.Open();

            //logowanie bez sida
            string sid = client.Zaloguj("123456789");

            using (new OperationContextScope(client.InnerChannel))
            {
                var requestMessage = new System.ServiceModel.Channels.HttpRequestMessageProperty();
                requestMessage.Headers["sid"] = sid;
                OperationContext.Current.OutgoingMessageProperties[System.ServiceModel.Channels.HttpRequestMessageProperty.Name] = requestMessage;

                var paramGR1 = new UslugaBIRzewnPublClient();
                
            }
//'DANE SZUKAJ 1
//Dim objParametryGR1 As New UslugaBIRpubl.ParametryWyszukiwania
//objParametryGR1.Nip = "1111111111"
//txtResult.Text &= "### DaneSzukaj (grupa 1):" & vbCrLf &
//cc.DaneSzukaj(objParametryGR1) & vbCrLf & vbCrLf
//''DANE POBIERZ PEŁNY RAPORT
//txtResult.Text &= "### DanePobierzPelnyRaport:" & vbCrLf &
//cc.DanePobierzPelnyRaport("39002176400000", "PublDaneRaportPrawna", "") & vbCrLf &
//vbCrLf
//'WYLOGUJ
//txtResult.Text &= "### Wyloguj:" & vbCrLf & cc.Wyloguj(strSID) & vbCrLf &
//vbCrLf
//'GET VALUE
//txtResult.Text &= "### GetValue(KomunikatKod):" & vbCrLf &
//cc.GetValue("KomunikatKod") & vbCrLf
//txtResult.Text &= "### GetValue(KomunikatTresc):" & vbCrLf &
//cc.GetValue("KomunikatTresc") & vbCrLf
//txtResult.Text &= "### GetValue(StatusSesji):" & vbCrLf &
//cc.GetValue("StatusSesji") & vbCrLf
//End Using
//cc.Close()
//End Sub
        }

    }
}
