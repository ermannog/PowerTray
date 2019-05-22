<#
.SYNOPSIS
  Name: IPv4Info.ps1
  Info on IPv4 settings

  .DESCRIPTION
  Get IPv4 WellKnow addresses of the system

  .NOTES
  Version:        1.0
  Author:         Ermanno Goletto>
  Creation Date:  2019-05-22
#>

$IPv4Adresses=Get-NetIPAddress -AddressFamily IPv4 | Where-Object PrefixOrigin -ne 'WellKnow'
$IPv4Adresses | Format-Table @{Label='C1'; Expression={$_.InterfaceAlias}}, `
	@{Label='C2'; Expression={'['+ $_.PrefixOrigin+']'}; Alignment='Left'}, `
	@{Label='C3'; Expression={': ' + $_.IPAddress+'/'+$_.PrefixLength}} `
	-HideTableHeaders -AutoSize

# Get-NetIPAddress -AddressFamily IPv4 | Where-Object PrefixOrigin -ne 'WellKnow'| Format-Table InterfaceAlias, @{Name='Origin'; Expression={'['+ $_.PrefixOrigin+']'}; Alignment='Left'}, {': ' + $_.IPAddress+'/'+$_.PrefixLength} -HideTableHeaders
# Get-NetIPAddress -AddressFamily IPv4 | Where-Object PrefixOrigin -ne 'WellKnow' | Select-Object @{name='Address'; expression={$_.IPAddress+'/ '+$_.PrefixLength+' ['+$_.PrefixOrigin+']'}}, @{name='Interface'; expression={$_.InterfaceAlias}}, @{name='State'; expression={$_.AddressState}}
# Get-NetIPAddress -AddressFamily IPv4 | Where-Object PrefixOrigin -ne 'WellKnow'| Format-Table InterfaceAlias, @{Expression={'['+$_.PrefixOrigin+']'}; Alignment='Left'}, {': ' + $_.IPAddress+'/'+$_.PrefixLength} -HideTableHeaders