# Prompt Pay (EMVCo)
ตัวช่วยในการ `อ่าน` และ `สร้าง` QR Prompt Pay ตามมาตรฐานของธนาคารแห่งประเทศไทย **BOT** (Bank of Thailand) โดยอ้างอิงจากเอกสารสำคัญ 3 ตัวด้านล่างนี้
1. [Thai QR Code - Payment Standard](https://www.bot.or.th/Thai/PaymentSystems/StandardPS/Documents/ThaiQRCode_Payment_Standard.pdf)
1. [ENVCo Consumer Presented Mode](https://www.emvco.com/wp-content/plugins/pmpro-customizations/oy-getfile.php?u=/wp-content/uploads/documents/EMVCo-Consumer-Presented-QR-Specification-v1-1.pdf)
1. [EMVCo Merchant-Presented Mode](https://www.emvco.com/wp-content/plugins/pmpro-customizations/oy-getfile.php?u=/wp-content/uploads/documents/EMVCo-Merchant-Presented-QR-Specification-v1-1.pdf)

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
เป็นการโอนเงินระหว่างประชาชนทั่วไปไม่เกี่ยวกับธุรกิจหรือบริษัทใดๆ โดยการสร้าง QR ประเภทนี้ จะต้องเรียกใช้ผ่านเมธอด `GetCreditTransferQR()` ตามโค้ดด้านล่างนี้
```csharp
// สร้าง QR โอนเงินสำหรับบุคคลทั่วไป
var qr = PPay.StaticQR.GetCreditTransferQR();
```

### การระบุผู้รับเงิน
ตามมาตรฐานของธนาคารแห่งประเทศไทย เราสามารถระบุผู้รับเงินได้ 4 วิธีคือ `เบอร์มือถือ`, `เลขประจำตัวประชาชน`, `เลขบัญชีธนาคาร` และ `e-wallet` ตามโค้ดตัวอย่างด้านล่างนี้

1. ระบุผู้รับเงินด้วย `เบอร์มือถือ`
```csharp
// โอนเงินพร้อมเพย์ไปที่ เบอร์มือถือ 091-418-5401 (จำนวนเงินที่จะโอนผู้ใช้ต้องกรอกเอง)
var qr = PPay.StaticQR.MobileNumber("0914185401").GetCreditTransferQR();
```

2. ระบุผู้รับเงินด้วย `เลขประจำตัวประชาชน`
```csharp
// โอนเงินพร้อมเพย์ไปที่ เลขประจำตัวประชาชน 0-0000-00000-00-0 (จำนวนเงินที่จะโอนผู้ใช้ต้องกรอกเอง)
var qr = PPay.StaticQR.NationalId("0000000000000").GetCreditTransferQR();
```

3. ระบุผู้รับเงินด้วย `เลขบัญชีธนาคาร`
```csharp
// โอนเงินพร้อมเพย์ไปที่ เลขบัญชีธนาคาร 0000000000 (จำนวนเงินที่จะโอนผู้ใช้ต้องกรอกเอง)
var qr = PPay.StaticQR.BankAccount("000000000000000").GetCreditTransferQR();
```

4. ระบุผู้รับเงินด้วย `e-wallet`
```csharp
// โอนเงินพร้อมเพย์ไปที่ e-Wallet Id 000000000000000 (จำนวนเงินที่จะโอนผู้ใช้ต้องกรอกเอง)
var qr = PPay.StaticQR.EWallet("000000000000000").GetCreditTransferQR();
```

### การกำหนดจำนวนเงินที่ต้องจ่าย
เราสามารถสร้าง QR ที่มีการกำหนดเงินที่ต้องจ่ายเป็นค่าตายตัวได้เลย โดยการเรียกใช้เมธอด `Amount()` ตามโค้ดตัวอย่างด้านล่าง
```csharp
// โอนเงินพร้อมเพย์ไปที่ เบอร์มือถือ 091-418-5401 จำนวน 50 บาท
var qr = PPay.StaticQR.MobileNumber("0914185401").Amount(50).GetCreditTransferQR();

// โอนเงินพร้อมเพย์ไปที่ เลขประจำตัวประชาชน 0-0000-00000-00-0 จำนวน 50 บาท
var qr = PPay.StaticQR.NationalId("0000000000000").Amount(50).GetCreditTransferQR();


// โอนเงินพร้อมเพย์ไปที่ เลขบัญชีธนาคาร 0000000000 จำนวน 50 บาท
var qr = PPay.StaticQR.BankAccount("000000000000000").Amount(50).GetCreditTransferQR();

// โอนเงินพร้อมเพย์ไปที่ e-Wallet Id 000000000000000 จำนวน 50 บาท
var qr = PPay.StaticQR.EWallet("000000000000000").Amount(50).GetCreditTransferQR();
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
เป็นการสร้าง QR สำหรับเรียกเก็บเงินจาก ร้านค้า/บริษัท โดยการสร้าง QR ประเภทนี้ จะต้องเรียกใช้ผ่านเมธอด `GetBillPaymentQR()` ตามโค้ดด้านล่างนี้
```csharp
// สร้าง QR สำหรับธุรกิจ
var qr = PPay.StaticQR.GetBillPaymentQR();
```

### การระบุผู้รับเงิน
ตามมาตรฐานของธนาคารแห่งประเทศไทย ตัว QR ประเภทนี้สามารถกำหนดผู้รับเงินได้ 2 วิธีคือ `รหัสประจำตัวประชาชน` หรือ `เลขประจำตัวผู้เสียภาษี` ตามด้วยรหัสร้านสาขา (suffix) ต่อท้าย 2 ตัว และรหัสอ้างอิงเพื่อเอาไว้ยืนยันกับธนาคารกรณีมีปัญหา ตามโค้ดตัวอย่างด้านล่างนี้

1. ระบุผู้รับเงินด้วย `รหัสประจำตัวประชาชน`
```csharp
// จ่ายเงินพร้อมเพย์ไปที่ รหัสประชาชน 0-0000-00000-00-0 รหัสร้านสาขา 99 (2 หลัก)
// รหัสอ้างอิง 1: 1234, รหัสอ้างอิง 2: 5678
var qr = PPay.StaticQR
    .NationalId("0000000000000")
    .BillerSuffix("99")
    .BillRef1("1234")
    .BillRef2("5678")
    .GetBillPaymentQR();
```

2. ระบุผู้รับเงินด้วย `เลขประจำตัวผู้เสียภาษี`
```csharp
// จ่ายเงินพร้อมเพย์ไปที่ เลขประจำตัวผู้เสียภาษี 0000000000000 รหัสร้านสาขา 99 (2 หลัก)
// รหัสอ้างอิง 1: 1234, รหัสอ้างอิง 2: 5678 (จำนวนเงินที่จะโอนผู้ใช้ต้องกรอกเอง)
var qr = PPay.StaticQR
    .TaxId("000000000000099")
    .BillerSuffix("99")
    .BillRef1("1234")
    .BillRef2("5678")
    .GetBillPaymentQR();
```

### การกำหนดจำนวนเงินที่ต้องจ่าย
เราสามารถสร้าง QR ที่มีการกำหนดเงินที่ต้องจ่ายเป็นค่าตายตัวได้เลย โดยการเรียกใช้เมธอด `Amount()` ตามโค้ดตัวอย่างด้านล่าง
```csharp
// จ่ายเงินพร้อมเพย์ไปที่ เลขประจำตัวผู้เสียภาษี 0000000000000 รหัสร้านสาขา 99 (2 หลัก)
// รหัสอ้างอิง 1: 1234, รหัสอ้างอิง 2: 5678 จำนวน 50 บาท
var qr = PPay.StaticQR
    .TaxId("000000000000099")
    .BillerSuffix("99")
    .BillRef1("1234")
    .BillRef2("5678")
    .Amount(50)
    .GetBillPaymentQR();
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

## Progress
|Feature|สถานะ|หมายเหตุ|
|--|--|--|
|สร้าง Bill Payment|ทำงานได้|ไม่มี Test cases cover|
|สร้าง Transfer with PromptPayID|ทำงานได้|ไม่มี Test cases cover + ตัดเบอร์โทรกากๆไปก่อน|
|อ่าน QR text แปลงเป็น model|ยังไม่ได้ทำ||
