/* Comportamiento comun de las vistas Index (una sola vista por entidad):
   Nuevo / Editar / Eliminar / Detalle se resuelven con modales de Bootstrap.
   Cada <tr> trae su registro en el atributo data-item (JSON). */
(function (window, document) {
    'use strict';

    var cfg = { title: '', detail: [] };
    var wired = false;

    function pad(n) { return n < 10 ? '0' + n : '' + n; }

    function nowLocal() {
        var d = new Date();
        return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()) +
            'T' + pad(d.getHours()) + ':' + pad(d.getMinutes());
    }

    function rowItem(btn) {
        return JSON.parse(btn.closest('tr').getAttribute('data-item'));
    }

    function showModal(id) {
        bootstrap.Modal.getOrCreateInstance(document.getElementById(id)).show();
    }

    function fillForm(form, item) {
        form.reset();
        Array.prototype.forEach.call(form.elements, function (el) {
            if (!el.name || !Object.prototype.hasOwnProperty.call(item, el.name)) { return; }
            var v = item[el.name];
            if (el.type === 'checkbox') {
                el.checked = (v === true);
            } else {
                el.value = (v === null || v === undefined) ? '' : v;
            }
        });
    }

    function format(value, type) {
        if (value === null || value === undefined || value === '') { return '-'; }
        if (type === 'bool') { return value ? 'Si' : 'No'; }
        if (type === 'datetime') { return String(value).replace('T', ' '); }
        if (type === 'money') { return Number(value).toFixed(2); }
        return String(value);
    }

    function openCreate() {
        var form = document.getElementById('createModalForm');
        form.reset();
        Array.prototype.forEach.call(form.querySelectorAll('[data-default="now"]'), function (el) {
            el.value = nowLocal();
        });
        showModal('createModal');
    }

    function openEdit(btn) {
        var item = rowItem(btn);
        fillForm(document.getElementById('editModalForm'), item);
        showModal('editModal');
    }

    function openDetail(btn) {
        var item = rowItem(btn);
        var body = document.getElementById('detailBody');
        body.innerHTML = '';
        cfg.detail.forEach(function (d) {
            var dt = document.createElement('dt');
            dt.className = 'col-sm-4';
            dt.textContent = d.label;
            var dd = document.createElement('dd');
            dd.className = 'col-sm-8';
            dd.textContent = format(item[d.key], d.type);
            body.appendChild(dt);
            body.appendChild(dd);
        });
        document.getElementById('detailModalTitle').textContent = 'Detalle: ' + cfg.title;
        showModal('detailModal');
    }

    function openDelete(btn) {
        var item = rowItem(btn);
        var holder = document.getElementById('deleteKeys');
        holder.innerHTML = '';
        var parts = [];
        Object.keys(item.__keys).forEach(function (k) {
            var input = document.createElement('input');
            input.type = 'hidden';
            input.name = k;
            input.value = item.__keys[k];
            holder.appendChild(input);
            parts.push(k + ' ' + item.__keys[k]);
        });
        document.getElementById('deleteText').textContent =
            'Seguro que desea eliminar este registro (' + cfg.title + ': ' + parts.join(', ') + ')? Esta accion no se puede deshacer.';
        showModal('deleteModal');
    }

    function wire() {
        if (wired) { return; }
        wired = true;

        document.addEventListener('click', function (e) {
            var btn = e.target.closest ? e.target.closest('[data-crud]') : null;
            if (!btn) { return; }
            switch (btn.getAttribute('data-crud')) {
                case 'create': openCreate(); break;
                case 'edit': openEdit(btn); break;
                case 'detail': openDetail(btn); break;
                case 'delete': openDelete(btn); break;
            }
        });

        var filter = document.getElementById('crudFilter');
        if (filter) {
            filter.addEventListener('input', function () {
                var q = filter.value.toLowerCase();
                Array.prototype.forEach.call(document.querySelectorAll('#crudTable tbody tr[data-item]'), function (tr) {
                    tr.style.display = tr.textContent.toLowerCase().indexOf(q) === -1 ? 'none' : '';
                });
            });
        }
    }

    window.Crud = {
        init: function (config) {
            cfg = config || cfg;
            wire();
        }
    };
})(window, document);
