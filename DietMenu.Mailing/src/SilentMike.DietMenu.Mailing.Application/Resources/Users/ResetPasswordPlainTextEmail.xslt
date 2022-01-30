<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:output method="text" indent="yes" omit-xml-declaration="yes" />
	
  <xsl:template match="SendResetPasswordEmail">
	<xsl:text>&#10;</xsl:text>
	<xsl:text>Hello,</xsl:text>
	<xsl:text>&#10;</xsl:text>
	  <xsl:text>We've received a request to reset the password for the DietMenu account associated with </xsl:text><xsl:value-of select="Email"/><xsl:text>.</xsl:text>
	  <xsl:text>&#10;</xsl:text>
	  <xsl:text>No changes have been made to your account yet</xsl:text>
	  <xsl:text>&#10;</xsl:text>
	  <xsl:text>You can reset your password by visiting the link: </xsl:text><xsl:value-of select="Url"/>
	  <xsl:text>&#10;</xsl:text>
	  <xsl:text>If you did not request a new password, please let us know immediately by replying to this email.</xsl:text>
	  <xsl:text>&#10;</xsl:text>
	<xsl:text>-- The DietMenu team</xsl:text>
  </xsl:template>
</xsl:stylesheet>