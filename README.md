# Prompt Pay (EMVCo)
ตัวช่วยในการ `อ่าน` และ `สร้าง` QR Prompt Pay ตามมาตรฐานของธนาคารแห่งประเทศไทย **BOT** (Bank of Thailand) โดยอ้างอิงจากเอกสารสำคัญ 2 ด้านล่างนี้
1. [ENVCo Merchant Presented QR](https://www.emvco.com/wp-content/plugins/pmpro-customizations/oy-getfile.php?u=/wp-content/uploads/documents/EMVCo-Consumer-Presented-QR-Specification-v1-1.pdf)
1. [Thai QR Code - Payment Standard](https://www.bot.or.th/Thai/PaymentSystems/StandardPS/Documents/ThaiQRCode_Payment_Standard.pdf)

> ใครอยากเอาไปปู้ยี้ปู้ยำอะไรก็ตามสบาย ถ้าทำแล้วดีหรือเจอจุดผิดก็ฝาก `pull-request` เข้ามาด้วยจะเป็นประคุณมาก
> โค้ดตัวนี้ต้องใช้ **.NET Core version 3.0 ขึ้นไป** นะจ๊ะ

## การใช้งาน
QR ตามมาตรฐานของ EMVCo ได้แบ่งไว้ 2 ลักษณะการใช้งานคือ
1. Static QR - เป็น QR ที่ใช้จ่ายเงินได้หลายครั้ง
1. Dynamic QR -เป็น QR ที่ใช้ครั้งเดียวแล้วทิ้ง
สำหรับโค้ดในการสร้าง QR แต่ละรูปแบบก็ตามด้านล่างนี่แหละ
```csharp
// Static QR
QrBuilder builder = PPay.StaticQR;

// Dynamic QR
QrBuilder builder = PPay.DynamicQR;
```

## สร้าง QR โอนเงินสำหรับบุคคลทั่วไป (Credit Transfer - Tag 29)
ระบุจำนวนเงินตายตัว
```csharp
// โอนเงินพร้อมเพย์ไปที่ เบอร์ 091-418-5401 จำนวน 50 บาท
var qr = PPay.StaticQR.MobileNumber("0914185401").Amount(50).GetCreditTransferQR();

// โอนเงินพร้อมเพย์ไปที่ เลขประจำตัวประชาชน 0-0000-00000-00-0 จำนวน 50 บาท
var qr = PPay.StaticQR..NationalId("0000000000000").Amount(50).GetCreditTransferQR();

// โอนเงินพร้อมเพย์ไปที่ e-Wallet Id 000000000000000 จำนวน 50 บาท
var qr = PPay.StaticQR.EWallet("000000000000000").Amount(50).GetCreditTransferQR();
```

ผู้ใช้เลือกจำนวนเงินที่จะโอนเองได้
```csharp
// โอนเงินพร้อมเพย์ไปที่ เบอร์ 091-418-5401
var qr = PPay.StaticQR.MobileNumber("0914185401").GetCreditTransferQR();

// โอนเงินพร้อมเพย์ไปที่ เลขประจำตัวประชาชน 0-0000-00000-00-0
var qr = PPay.StaticQR..NationalId("0000000000000").GetCreditTransferQR();

// โอนเงินพร้อมเพย์ไปที่ e-Wallet Id 000000000000000
var qr = PPay.StaticQR.EWallet("000000000000000").GetCreditTransferQR();
```

### เพิ่มเติม
กรณีที่เป็น QR ประเภท Merchant Presented QR สามารถกำหนดโดยเรียกใช้เมธอด `MerchantPresentedQR()`
```csharp
var qr = PPay.StaticQR.MerchantPresentedQR().GetCreditTransferQR();
```

กรณีที่เป็น QR ประเภท Customer Presented QR สามารถกำหนดโดยเรียกใช้เมธอด `CustomerPresentedQR()`
```csharp
var qr = PPay.StaticQR.CustomerPresentedQR().GetCreditTransferQR();
```

## สร้าง QR สำหรับธุรกิจ (Bill Payment - Tag 30)
ระบุจำนวนเงินตายตัว
```csharp
// จ่ายเงินพร้อมเพย์ไปที่ เลขประจำตัวผู้เสียภาษี 0000000000000 + 99 (Suffix 2 หลัก)
// รหัสอ้างอิง 1: 1234, รหัสอ้างอิง 2: 5678, จำนวน 50 บาท
var qr = PPay.StaticQR.TaxId("000000000000099").BillerSuffix("02").BillRef1("1234").BillRef2("5678").Amount(50).GetBillPaymentQR();
```

ผู้ใช้เลือกจำนวนเงินที่จะจ่ายเองได้
```csharp
// จ่ายเงินพร้อมเพย์ไปที่ เลขประจำตัวผู้เสียภาษี 0000000000000 + 99 (Suffix 2 หลัก)
// รหัสอ้างอิง 1: 1234, รหัสอ้างอิง 2: 5678
var qr = PPay.StaticQR.TaxId("000000000000099").BillerSuffix("02").BillRef1("1234").BillRef2("5678").GetBillPaymentQR();
```

### เพิ่มเติม
กรณีที่เป็น QR ประเภทใช้ Domestic Merchant สามารถกำหนดโดยเรียกใช้เมธอด `DomesticMerchant()`
```csharp
var qr = PPay.StaticQR.DomesticMerchant().GetBillPaymentQR();
```

กรณีที่เป็น QR ประเภทใช้ Cross-Border Merchant สามารถกำหนดโดยเรียกใช้เมธอด `CrossBorderMerchant()`
```csharp
var qr = PPay.StaticQR.CrossBorderMerchant().GetBillPaymentQR();
```
