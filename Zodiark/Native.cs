using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Zodiark
{
	public static class Kernel32
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct LUID
		{
			public uint LowPart;
			public int HighPart;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct PRIVILEGE_SET
		{
			public uint PrivilegeCount;
			public uint Control;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
			public LUID_AND_ATTRIBUTES[] Privilege;
		}

		public struct LUID_AND_ATTRIBUTES
		{
			public LUID Luid;
			public uint Attributes;
		}

		[Flags]
		public enum MEM_ALLOCATION_TYPE : uint
		{
			/// <summary>
			/// Allocates memory charges (from the overall size of memory and the paging files on disk) for the specified reserved memory pages. The function
			/// also guarantees that when the caller later initially accesses the memory, the contents will be zero. Actual physical pages are not allocated
			/// unless/until the virtual addresses are actually accessed.To reserve and commit pages in one step, call VirtualAlloc with .Attempting to commit a
			/// specific address range by specifying MEM_COMMIT without MEM_RESERVE and a non-NULL lpAddress fails unless the entire range has already been
			/// reserved. The resulting error code is ERROR_INVALID_ADDRESS.An attempt to commit a page that is already committed does not cause the function to
			/// fail. This means that you can commit pages without first determining the current commitment state of each page.If lpAddress specifies an address
			/// within an enclave, flAllocationType must be MEM_COMMIT.
			/// </summary>
			MEM_COMMIT = 4096, // 0x00001000
			/// <summary>
			/// Reserves a range of the process's virtual address space without allocating any actual physical storage in memory or in the paging file on
			/// disk.You can commit reserved pages in subsequent calls to the VirtualAlloc function. To reserve and commit pages in one step, call VirtualAlloc
			/// with MEM_COMMIT | MEM_RESERVE.Other memory allocation functions, such as malloc and LocalAlloc, cannot use a reserved range of memory until it is released.
			/// </summary>
			MEM_RESERVE = 8192, // 0x00002000
			/// <summary>
			/// Decommits the specified region of committed pages. After the operation, the pages are in the reserved state. The function does not fail if you
			/// attempt to decommit an uncommitted page. This means that you can decommit a range of pages without first determining the current commitment
			/// state.Do not use this value with MEM_RELEASE.The MEM_DECOMMIT value is not supported when the lpAddress parameter provides the base address for
			/// an enclave.
			/// </summary>
			MEM_DECOMMIT = 16384, // 0x00004000
			/// <summary>
			/// Releases the specified region of pages. After this operation, the pages are in the free state. If you specify this value, dwSize must be 0
			/// (zero), and lpAddress must point to the base address returned by the VirtualAlloc function when the region is reserved. The function fails if
			/// either of these conditions is not met. If any pages in the region are committed currently, the function first decommits, and then releases
			/// them.The function does not fail if you attempt to release pages that are in different states, some reserved and some committed. This means that
			/// you can release a range of pages without first determining the current commitment state.Do not use this value with MEM_DECOMMIT.
			/// </summary>
			MEM_RELEASE = 32768, // 0x00008000
			/// <summary>
			/// Indicates free pages not accessible to the calling process and available to be allocated. For free pages, the information in the AllocationBase,
			/// AllocationProtect, Protect, and Type members is undefined.
			/// </summary>
			MEM_FREE = 65536, // 0x00010000
			/// <summary>Indicates that the memory pages within the region are private (that is, not shared by other processes).</summary>
			MEM_PRIVATE = 131072, // 0x00020000
			/// <summary>Indicates that the memory pages within the region are mapped into the view of a section.</summary>
			MEM_MAPPED = 262144, // 0x00040000
			/// <summary>
			/// Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest. The pages should not be read from or written
			/// to the paging file. However, the memory block will be used again later, so it should not be decommitted. This value cannot be used with any other
			/// value.Using this value does not guarantee that the range operated on with MEM_RESET will contain zeros. If you want the range to contain zeros,
			/// decommit the memory and then recommit it.When you specify MEM_RESET, the VirtualAlloc function ignores the value of flProtect. However, you must
			/// still set flProtect to a valid protection value, such as PAGE_NOACCESS.VirtualAlloc returns an error if you use MEM_RESET and the range of memory
			/// is mapped to a file. A shared view is only acceptable if it is mapped to a paging file.
			/// </summary>
			MEM_RESET = 524288, // 0x00080000
			/// <summary>Allocates memory at the highest possible address. This can be slower than regular allocations, especially when there are many allocations.</summary>
			MEM_TOP_DOWN = 1048576, // 0x00100000
			/// <summary>
			/// Causes the system to track pages that are written to in the allocated region. If you specify this value, you must also specify MEM_RESERVE.To
			/// retrieve the addresses of the pages that have been written to since the region was allocated or the write-tracking state was reset, call the
			/// GetWriteWatch function. To reset the write-tracking state, call GetWriteWatch or ResetWriteWatch. The write-tracking feature remains enabled for
			/// the memory region until the region is freed.
			/// </summary>
			MEM_WRITE_WATCH = 2097152, // 0x00200000
			/// <summary>
			/// Reserves an address range that can be used to map Address Windowing Extensions (AWE) pages.This value must be used with MEM_RESERVE and no other values.
			/// </summary>
			MEM_PHYSICAL = 4194304, // 0x00400000
			/// <summary></summary>
			MEM_ROTATE = 8388608, // 0x00800000
			/// <summary></summary>
			MEM_DIFFERENT_IMAGE_BASE_OK = MEM_ROTATE, // 0x00800000
			/// <summary>
			/// MEM_RESET_UNDO should only be called on an address range to which MEM_RESET was successfully applied earlier. It indicates that the data in the
			/// specified memory range specified by lpAddress and dwSize is of interest to the caller and attempts to reverse the effects of MEM_RESET. If the
			/// function succeeds, that means all data in the specified address range is intact. If the function fails, at least some of the data in the address
			/// range has been replaced with zeroes.This value cannot be used with any other value. If MEM_RESET_UNDO is called on an address range which was not
			/// MEM_RESET earlier, the behavior is undefined. When you specify MEM_RESET, the VirtualAlloc function ignores the value of flProtect. However, you
			/// must still set flProtect to a valid protection value, such as PAGE_NOACCESS.Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows
			/// Vista, Windows Server 2003 and Windows XP: The MEM_RESET_UNDO flag is not supported until Windows 8 and Windows Server 2012.
			/// </summary>
			MEM_RESET_UNDO = 16777216, // 0x01000000
			/// <summary>
			/// Allocates memory using large page support.The size and alignment must be a multiple of the large-page minimum. To obtain this value, use the
			/// GetLargePageMinimum function.If you specify this value, you must also specify MEM_RESERVE and MEM_COMMIT.
			/// </summary>
			MEM_LARGE_PAGES = 536870912, // 0x20000000
			/// <summary></summary>
			MEM_4MB_PAGES = 2147483648, // 0x80000000
			/// <summary></summary>
			MEM_64K_PAGES = MEM_LARGE_PAGES | MEM_PHYSICAL, // 0x20400000
		}

		[Flags]
		public enum MEM_PROTECTION : uint
		{
			/// <summary>
			/// Disables all access to the committed region of pages. An attempt to read from, write to, or execute the committed region results in an access violation.
			/// <para>This flag is not supported by the CreateFileMapping function.</para>
			/// </summary>
			PAGE_NOACCESS = 1,
			/// <summary>
			/// Enables read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation. If Data
			/// Execution Prevention is enabled, an attempt to execute code in the committed region results in an access violation.
			/// </summary>
			PAGE_READONLY = 2,
			/// <summary>
			/// Enables read-only or read/write access to the committed region of pages. If Data Execution Prevention is enabled, attempting to execute code in
			/// the committed region results in an access violation.
			/// </summary>
			PAGE_READWRITE = 4,
			/// <summary>
			/// Enables read-only or copy-on-write access to a mapped view of a file mapping object. An attempt to write to a committed copy-on-write page
			/// results in a private copy of the page being made for the process. The private page is marked as PAGE_READWRITE, and the change is written to the
			/// new page. If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.
			/// <para>This flag is not supported by the VirtualAlloc or VirtualAllocEx functions.</para>
			/// </summary>
			PAGE_WRITECOPY = 8,
			/// <summary>
			/// Enables execute access to the committed region of pages. An attempt to write to the committed region results in an access violation.
			/// <para>This flag is not supported by the CreateFileMapping function.</para>
			/// </summary>
			PAGE_EXECUTE = 16, // 0x00000010
			/// <summary>
			/// Enables execute or read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation.
			/// <para>
			/// Windows Server 2003 and Windows XP: This attribute is not supported by the CreateFileMapping function until Windows XP with SP2 and Windows
			/// Server 2003 with SP1.
			/// </para>
			/// </summary>
			PAGE_EXECUTE_READ = 32, // 0x00000020
			/// <summary>
			/// Enables execute, read-only, or read/write access to the committed region of pages.
			/// <para>
			/// Windows Server 2003 and Windows XP: This attribute is not supported by the CreateFileMapping function until Windows XP with SP2 and Windows
			/// Server 2003 with SP1.
			/// </para>
			/// </summary>
			PAGE_EXECUTE_READWRITE = 64, // 0x00000040
			/// <summary>
			/// Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object. An attempt to write to a committed copy-on-write
			/// page results in a private copy of the page being made for the process. The private page is marked as PAGE_EXECUTE_READWRITE, and the change is
			/// written to the new page.
			/// <para>This flag is not supported by the VirtualAlloc or VirtualAllocEx functions.</para>
			/// <para>
			/// Windows Vista, Windows Server 2003 and Windows XP: This attribute is not supported by the CreateFileMapping function until Windows Vista with SP1
			/// and Windows Server 2008.
			/// </para>
			/// </summary>
			PAGE_EXECUTE_WRITECOPY = 128, // 0x00000080
			/// <summary>
			/// Pages in the region become guard pages. Any attempt to access a guard page causes the system to raise a STATUS_GUARD_PAGE_VIOLATION exception and
			/// turn off the guard page status. Guard pages thus act as a one-time access alarm. For more information, see Creating Guard Pages.
			/// <para>When an access attempt leads the system to turn off guard page status, the underlying page protection takes over.</para>
			/// <para>If a guard page exception occurs during a system service, the service typically returns a failure status indicator.</para>
			/// <para>This value cannot be used with PAGE_NOACCESS.</para>
			/// <para>This flag is not supported by the CreateFileMapping function.</para>
			/// </summary>
			PAGE_GUARD = 256, // 0x00000100
			/// <summary>
			/// Sets all pages to be non-cachable. Applications should not use this attribute except when explicitly required for a device. Using the interlocked
			/// functions with memory that is mapped with SEC_NOCACHE can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.
			/// <para>The PAGE_NOCACHE flag cannot be used with the PAGE_GUARD, PAGE_NOACCESS, or PAGE_WRITECOMBINE flags.</para>
			/// <para>
			/// The PAGE_NOCACHE flag can be used only when allocating private memory with the VirtualAlloc, VirtualAllocEx, or VirtualAllocExNuma functions. To
			/// enable non-cached memory access for shared memory, specify the SEC_NOCACHE flag when calling the CreateFileMapping function.
			/// </para>
			/// </summary>
			PAGE_NOCACHE = 512, // 0x00000200
			/// <summary>
			/// Sets all pages to be write-combined.
			/// <para>
			/// Applications should not use this attribute except when explicitly required for a device. Using the interlocked functions with memory that is
			/// mapped as write-combined can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.
			/// </para>
			/// <para>The PAGE_WRITECOMBINE flag cannot be specified with the PAGE_NOACCESS, PAGE_GUARD, and PAGE_NOCACHE flags.</para>
			/// <para>
			/// The PAGE_WRITECOMBINE flag can be used only when allocating private memory with the VirtualAlloc, VirtualAllocEx, or VirtualAllocExNuma
			/// functions. To enable write-combined memory access for shared memory, specify the SEC_WRITECOMBINE flag when calling the CreateFileMapping function.
			/// </para>
			/// <para>Windows Server 2003 and Windows XP: This flag is not supported until Windows Server 2003 with SP1.</para>
			/// </summary>
			PAGE_WRITECOMBINE = 1024, // 0x00000400
			/// <summary>The page contents that you supply are excluded from measurement with the EEXTEND instruction of the Intel SGX programming model.</summary>
			PAGE_ENCLAVE_UNVALIDATED = 536870912, // 0x20000000
			/// <summary>
			/// Sets all locations in the pages as invalid targets for CFG. Used along with any execute page protection like PAGE_EXECUTE, PAGE_EXECUTE_READ,
			/// PAGE_EXECUTE_READWRITE and PAGE_EXECUTE_WRITECOPY. Any indirect call to locations in those pages will fail CFG checks and the process will be
			/// terminated. The default behavior for executable pages allocated is to be marked valid call targets for CFG.
			/// <para>This flag is not supported by the VirtualProtect or CreateFileMapping functions.</para>
			/// </summary>
			PAGE_TARGETS_INVALID = 1073741824, // 0x40000000
			/// <summary>
			/// Pages in the region will not have their CFG information updated while the protection changes for VirtualProtect. For example, if the pages in the
			/// region was allocated using PAGE_TARGETS_INVALID, then the invalid information will be maintained while the page protection changes. This flag is
			/// only valid when the protection changes to an executable type like PAGE_EXECUTE, PAGE_EXECUTE_READ, PAGE_EXECUTE_READWRITE and
			/// PAGE_EXECUTE_WRITECOPY. The default behavior for VirtualProtect protection change to executable is to mark all locations as valid call targets
			/// for CFG.
			/// <para>The following are modifiers that can be used in addition to the options provided in the previous table, except as noted.</para>
			/// </summary>
			PAGE_TARGETS_NO_UPDATE = PAGE_TARGETS_INVALID, // 0x40000000
			/// <summary>The page contains a thread control structure (TCS).</summary>
			PAGE_ENCLAVE_THREAD_CONTROL = 2147483648, // 0x80000000
			/// <summary></summary>
			PAGE_REVERT_TO_FILE_MAP = PAGE_ENCLAVE_THREAD_CONTROL, // 0x80000000
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int processId);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern bool IsWow64Process(IntPtr hProcess, out bool lpSystemInfo);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [Out] byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesRead);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, byte[] lpBuffer, UIntPtr nSize, out IntPtr lpNumberOfBytesWritten);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int dwSize, out IntPtr lpNumberOfBytesWritten);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetCurrentProcess();

		[SuppressUnmanagedCodeSecurity]
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref LUID lpLuid);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool PrivilegeCheck(IntPtr clientToken, ref PRIVILEGE_SET requiredPrivileges, out bool pfResult);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool VirtualFree([In] IntPtr lpAddress, UIntPtr dwSize, MEM_ALLOCATION_TYPE dwFreeType);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool VirtualFreeEx([In] IntPtr hProcess, [In] IntPtr lpAddress, UIntPtr dwSize, MEM_ALLOCATION_TYPE dwFreeType);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr VirtualAlloc([In] IntPtr lpAddress, UIntPtr dwSize, MEM_ALLOCATION_TYPE flAllocationType, MEM_PROTECTION flProtect);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr VirtualAllocEx([In] IntPtr hProcess, [In] IntPtr lpAddress, UIntPtr dwSize, MEM_ALLOCATION_TYPE flAllocationType, Kernel32.MEM_PROTECTION flProtect);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		public static extern int WaitForSingleObject(IntPtr handle, UInt32 milliseconds);


		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		public static extern IntPtr GetExitCodeThread(IntPtr handle, out IntPtr lpExitCode);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern int CloseHandle(IntPtr hObject);

	}
}
