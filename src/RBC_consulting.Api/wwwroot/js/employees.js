const MAX_FILE_SIZE = 5 * 1024 * 1024;
const $ = id => document.getElementById(id);

let search = '', sortBy = 'EmployeeID', sortDir = 'asc', page = 1, pageSize = 10;
let debounce, chart;

document.addEventListener('DOMContentLoaded', () => {
    loadEmployees();
    loadStatistics();

    $('searchInput').addEventListener('input', e => {
        clearTimeout(debounce);
        debounce = setTimeout(() => { search = e.target.value; page = 1; loadEmployees(); }, 300);
    });

    document.querySelectorAll('.sortable').forEach(th => th.addEventListener('click', () => {
        const col = th.dataset.column;
        sortDir = sortBy === col && sortDir === 'asc' ? 'desc' : 'asc';
        sortBy = col;
        loadEmployees();
    }));
});

async function loadEmployees() {
    const tbody = $('employeesTableBody');
    try {
        const params = new URLSearchParams({ pageNumber: page, pageSize, searchTerm: search, sortBy, sortDir });
        const data = await fetch(`/api/Employees?${params}`).then(r => r.json());
        const items = data.items ?? data.Items ?? [];
        const total = data.totalCount ?? data.TotalCount ?? 0;

        tbody.innerHTML = items.length
            ? items.map(empRow).join('')
            : '<tr><td colspan="9" style="text-align:center;padding:48px;color:#94a3b8;font-size:.875rem;">No employees found</td></tr>';

        renderPagination(data);
        $('totalCount').textContent = total === 1 ? '1 employee' : `${total} employees`;
    } catch {
        tbody.innerHTML = '<tr><td colspan="9" style="text-align:center;padding:40px;color:#94a3b8;">Failed to load employees</td></tr>';
    }
}

function empRow(e) {
    const fileCell = e.hasFile
        ? `<a href="/api/employees/${e.id}/file" target="_blank" style="color:#10b981">${fileIcon()}</a>`
        : `<span style="color:#cbd5e1;font-size:.75rem;">none</span>`;
    return `<tr>
        <td class="text-sm" style="color:#94a3b8;">#${e.id}</td>
        <td><span class="fw-name">${esc(e.fullName)}</span></td>
        <td><span class="badge-pos">${esc(e.position)}</span></td>
        <td>${esc(e.department)}</td>
        <td class="text-sm">${fmtDate(e.hireDate)}</td>
        <td><div class="text-sm">${esc(e.email||'—')}</div><div class="text-sm">${esc(e.phone||'—')}</div></td>
        <td style="font-weight:600">${fmtCurrency(e.salary)}</td>
        <td>${fileCell}</td>
        <td style="text-align:right;white-space:nowrap;">
            <button class="act-btn" onclick="openEdit(${e.id})" title="Edit">${editIcon()}</button>
            <button class="act-btn danger" onclick="deleteEmployee(${e.id})" title="Delete">${deleteIcon()}</button>
        </td>
    </tr>`;
}

function renderPagination(data) {
    const p = data.pageNumber ?? data.PageNumber ?? page;
    const total = data.totalPages ?? data.TotalPages ?? 1;
    const prev = data.hasPreviousPage ?? data.HasPreviousPage ?? false;
    const next = data.hasNextPage ?? data.HasNextPage ?? false;

    let html = `<li class="${prev?'':'disabled'}"><a href="#" onclick="${prev?`changePage(${p-1})`:'return false'}">&#8592;</a></li>`;
    for (let i = 1; i <= total; i++) {
        if (i === 1 || i === total || Math.abs(i - p) <= 2)
            html += `<li class="${i===p?'active':''}"><a href="#" onclick="changePage(${i})">${i}</a></li>`;
        else if (Math.abs(i - p) === 3)
            html += `<li class="disabled"><span>…</span></li>`;
    }
    html += `<li class="${next?'':'disabled'}"><a href="#" onclick="${next?`changePage(${p+1})`:'return false'}">&#8594;</a></li>`;
    $('paginationControls').innerHTML = html;
}

function changePage(p) { page = p; loadEmployees(); }

async function loadStatistics() {
    try {
        const stats = await fetch('/api/employees/statistics').then(r => r.json());
        renderChart(stats);
    } catch { }
}

function renderChart(stats) {
    const ctx = $('employeeChart');
    if (!ctx) return;
    chart?.destroy();
    const colors = ['rgba(37,99,235,.85)','rgba(16,185,129,.85)','rgba(139,92,246,.85)','rgba(245,158,11,.85)','rgba(239,68,68,.85)','rgba(14,165,233,.85)','rgba(236,72,153,.85)','rgba(20,184,166,.85)'];
    chart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: stats.map(s => s.department),
            datasets: [{ label: 'Employees', data: stats.map(s => s.employeeCount), backgroundColor: stats.map((_, i) => colors[i % colors.length]), borderRadius: 6, borderSkipped: false }]
        },
        options: {
            responsive: true, maintainAspectRatio: false,
            plugins: { legend: { display: false }, tooltip: { callbacks: { label: c => ` ${c.parsed.y} employee${c.parsed.y !== 1 ? 's' : ''}` } } },
            scales: {
                x: { grid: { display: false }, ticks: { color: '#94a3b8', font: { size: 11 } } },
                y: { beginAtZero: true, grid: { color: '#f1f5f9' }, ticks: { color: '#94a3b8', font: { size: 11 }, stepSize: 1, precision: 0 } }
            }
        }
    });
}

function openCreate() {
    $('employeeForm').reset();
    $('employeeId').value = '';
    $('modalTitle').textContent = 'Add Employee';
    resetFileDrop();
    openModal();
}

async function openEdit(id) {
    try {
        const emp = await fetch(`/api/employees/${id}`).then(r => { if (!r.ok) throw new Error(); return r.json(); });
        $('employeeId').value = emp.id;
        $('fullName').value = emp.fullName;
        $('position').value = emp.position;
        $('department').value = emp.department;
        $('hireDate').value = emp.hireDate?.split('T')[0] ?? '';
        $('salary').value = emp.salary ?? '';
        $('email').value = emp.email ?? '';
        $('phone').value = emp.phone ?? '';
        $('modalTitle').textContent = 'Edit Employee';
        resetFileDrop();
        if (emp.hasFile) $('existingFileBar').style.display = 'flex';
        openModal();
    } catch {
        showToast('Failed to load employee', 'error');
    }
}

const openModal  = () => { $('modalBackdrop').classList.add('open'); $('sideModal').classList.add('open'); };
const closeModal = () => { $('modalBackdrop').classList.remove('open'); $('sideModal').classList.remove('open'); };

function onFileSelected(input) {
    if (!input.files.length) { resetFileDrop(); return; }
    const file = input.files[0];
    if (file.size > MAX_FILE_SIZE) { showToast('File exceeds 5 MB limit', 'error'); input.value = ''; return; }
    $('fileDrop').classList.add('has-file');
    $('fileLabel').textContent = file.name;
    $('existingFileBar').style.display = 'none';
    $('deleteFile').value = 'false';
}

function resetFileDrop() {
    $('fileDrop').classList.remove('has-file');
    $('fileLabel').textContent = 'Click to upload PDF, JPG or PNG';
    $('fileInput').value = '';
    $('existingFileBar').style.display = 'none';
    $('deleteFile').value = 'false';
}

function markFileForDeletion() {
    $('deleteFile').value = 'true';
    $('existingFileBar').style.display = 'none';
    $('fileDrop').classList.remove('has-file');
    $('fileLabel').textContent = 'Click to upload PDF, JPG or PNG';
    $('fileInput').value = '';
}

async function saveEmployee(event) {
    event.preventDefault();
    const id = $('employeeId').value;
    const payload = {
        fullName:   $('fullName').value,
        position:   $('position').value,
        department: $('department').value,
        hireDate:   $('hireDate').value,
        salary:     parseFloat($('salary').value) || null,
        email:      $('email').value || null,
        phone:      $('phone').value || null
    };

    const res = await fetch(id ? `/api/employees/${id}` : '/api/employees', {
        method: id ? 'PUT' : 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    }).catch(() => null);

    if (!res?.ok) {
        const err = await res?.json().catch(() => ({}));
        showToast(err?.message || 'Operation failed', 'error');
        return;
    }

    let empId = id || await res.json().catch(() => null);
    if (!empId) { closeModal(); loadDashboard(); showToast('Employee created (could not attach file)', 'success'); return; }

    const file = $('fileInput').files[0];
    const shouldDelete = $('deleteFile').value === 'true';

    if (file) {
        const fd = new FormData();
        fd.append('file', file);
        const up = await fetch(`/api/employees/${empId}/upload-file`, { method: 'POST', body: fd }).catch(() => null);
        if (!up?.ok) { closeModal(); loadDashboard(); showToast('Employee saved but file upload failed', 'error'); return; }
    } else if (shouldDelete) {
        const del = await fetch(`/api/employees/${empId}/file`, { method: 'DELETE' }).catch(() => null);
        if (!del?.ok) { closeModal(); loadDashboard(); showToast('Employee saved but file removal failed', 'error'); return; }
    }

    closeModal();
    loadDashboard();
    showToast(id ? 'Employee updated' : 'Employee created', 'success');
}

async function deleteEmployee(id) {
    if (!confirm('Delete this employee?')) return;
    const res = await fetch(`/api/employees/${id}`, { method: 'DELETE' }).catch(() => null);
    res?.ok ? (loadDashboard(), showToast('Employee deleted', 'success')) : showToast('Delete failed', 'error');
}

function exportPDF() {
    window.open(`/api/Employees/export-pdf?${new URLSearchParams({ searchTerm: search, sortBy, sortDir })}`, '_blank');
}

function loadDashboard() { loadEmployees(); loadStatistics(); }

const esc = t => { const d = document.createElement('div'); d.textContent = t || ''; return d.innerHTML; };
const fmtDate = s => s ? new Date(s).toLocaleDateString('en-US', { year:'numeric', month:'short', day:'numeric' }) : '—';
const fmtCurrency = v => v == null ? '—' : new Intl.NumberFormat('en-US', { style:'currency', currency:'USD', maximumFractionDigits:0 }).format(v);

let toastTimer;
function showToast(msg, type = 'success') {
    const el = $('toast');
    el.textContent = msg;
    el.className = `show ${type}`;
    clearTimeout(toastTimer);
    toastTimer = setTimeout(() => el.className = '', 3000);
}

function fileIcon() {
    return `<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" viewBox="0 0 16 16"><path d="M4 0h5.293A1 1 0 0 1 10 .293L13.707 4a1 1 0 0 1 .293.707V14a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V2a2 2 0 0 1 2-2m5.5 1.5v2a1 1 0 0 0 1 1h2z"/></svg>`;
}
function editIcon() {
    return `<svg xmlns="http://www.w3.org/2000/svg" width="15" height="15" fill="currentColor" viewBox="0 0 16 16"><path d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325"/></svg>`;
}
function deleteIcon() {
    return `<svg xmlns="http://www.w3.org/2000/svg" width="15" height="15" fill="currentColor" viewBox="0 0 16 16"><path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"/><path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"/></svg>`;
}
