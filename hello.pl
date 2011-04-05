#!/usr/bin/perl

package main;
use SOAP::Lite;

# Variables
my $url = 'http://localhost/WcfService1/Service1.svc?wsdl';
my $url_debug = 'http://localhost:11040/Service1.svc?wsdl';
my $uri = 'http://tempuri.org/';
my $xmlns = 'http://schemas.datacontract.org/2004/07/WcfService1';

# Setup Network Connection
my $soap = SOAP::Lite
-> uri($uri)
-> on_action(sub { sprintf '%sIService1/%s', @_ })
-> proxy($url)
->autotype(0)->readable(1);

# Call HelloWorld
my $response = $soap->HelloWorld(SOAP::Data->new(name => 'name', value => 'Johnny Five'));

# Print the Result
if ($response->fault)
{
  die $response->faultstring;
}
else
{
  print $response->result;
  print "\n";
}

# Call HelloWorld2
$response = $soap->HelloWorld2(SOAP::Data->new(name => 'nameData', value => [
   SOAP::Data->new(name => 'a:FirstName', value => 'Henry'),
   SOAP::Data->new(name => 'a:LastName', value => 'Higgens'),
   SOAP::Data->new(name => 'a:Age', value => '42')
   ])->attr( { 'xmlns:a' => $xmlns } )
);

# Print the Result
if ($response->fault)
{
  die $response->faultstring;
}
else
{
  print $response->result;
  print "\n";
}
